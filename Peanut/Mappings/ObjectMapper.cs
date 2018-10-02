using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Peanut.Mappings
{
    class ObjectMapper
    {
        public ObjectMapper(Type objtype)
        {
            
            Connection = new ConnectionAttribute(DB.DB1);
            ObjectType = objtype;
            OnInit(objtype);
        }
        private Dictionary<Type, DObjectToBObject> mDTBMap = new Dictionary<Type, DObjectToBObject>();
        private Dictionary<Type, SelectDataReader> mSelectReader = new Dictionary<Type, SelectDataReader>();
        private KeyValueCollection<PropertyMapper> mKeyByProperties = new KeyValueCollection<PropertyMapper>();
        public PropertyMapper this[string column]
        {
            get
            {
                return mKeyByProperties[column];
            }
        }

        public SelectChangeAttribute SelectChange
        {
            get;
            set;
        }

        private void OnInit(Type type)
        {
            TableAttribute[] ta = Utils.GetTypeAttributes<TableAttribute>(type, false);
            SelectChangeAttribute[] SCAS = Utils.GetTypeAttributes<SelectChangeAttribute>(type, true);
            if (SCAS.Length > 0)
                SelectChange = SCAS[0];
            IDAttribute[] id;
            ColumnAttribute[] col;
            AggregationAttribute[] aggr;
            PropertyCastAttribute[] pca;
            PropertyMapper pm;
            ValueAttribute[] val;
            RelationAttribute[] ra;
            if (ta.Length == 0)
                throw new PeanutException(string.Format(DataMsg.OBJECT_NOT_MAPTOTABLE, type.FullName));
            mTable= string.IsNullOrEmpty(ta[0].Name) ? ObjectType.Name : ta[0].Name;
            if (ta[0].DISTINCT)
            {
                DISTINCT = " DISTINCT ";
            }
            else
            {
                DISTINCT = " ";
            }
            Connection = new ConnectionAttribute(ta[0].DefaultConnection);
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                id = Utils.GetPropertyAttributes<IDAttribute>(pi, false);
                col = Utils.GetPropertyAttributes<ColumnAttribute>(pi, false);
                aggr = Utils.GetPropertyAttributes<AggregationAttribute>(pi, false);
                pca = Utils.GetPropertyAttributes<PropertyCastAttribute>(pi, false);
                val = Utils.GetPropertyAttributes<ValueAttribute>(pi, false);
                ra = Utils.GetPropertyAttributes<RelationAttribute>(pi, false);
                if (pca.Length == 0)
                    pca = Utils.GetTypeAttributes<PropertyCastAttribute>(pi.PropertyType, false);
                
                if (id.Length > 0)
                {
                    if (ID != null)
                        throw new PeanutException(string.Format(DataMsg.REPEATE_DESC_ID,type.FullName));
                    ID = new PropertyMapper();
                    ID.OM = this;
                    ID.Validaters = Utils.GetPropertyAttributes<Validates.ValidaterAttribute>(pi, false);
                    ID.ColumnName = string.IsNullOrEmpty(id[0].Name) ? pi.Name : id[0].Name;
                    ID.Handler = new PropertyHandler(pi);
                    if (pca.Length > 0)
                    {
                        ID.Cast = pca[0];
                        
                    }
                    if (val.Length > 0)
                        ID.Value = val[0];
                    mKeyByProperties.Add(ID.ColumnName,ID);
                   
                    foreach (RelationAttribute raitem in ra)
                    {
                        raitem.Owner = this;
                        raitem.OwnerProperty = ID;
                        Relations.Add(raitem.Partner, raitem);
                    }
                }
                if (col.Length > 0)
                {
                    pm = new PropertyMapper();
                    pm.ColumnName = string.IsNullOrEmpty(col[0].Name) ? pi.Name : col[0].Name;
                    pm.Handler = new PropertyHandler(pi);
                    pm.OM = this;
                    pm.Validaters = Utils.GetPropertyAttributes<Validates.ValidaterAttribute>(pi, false);
                    if (aggr.Length > 0)
                        pm.Aggregation = aggr[0];
                    if (pca.Length > 0)
                        pm.Cast = pca[0];
                    if (val.Length > 0)
                        pm.Value = val[0];
                    if (Properties.Contains(pm))
                        throw new PeanutException(string.Format(DataMsg.REPEATE_DESC_FIELD, type.FullName, pm.ColumnName));
                    Properties.Add(pm);
                    if (pm.Cast == null)
                        pm.Cast = DBContext.GetCast(pi.PropertyType);
                    mKeyByProperties.Add(pm.ColumnName, pm);
                  
                    foreach (RelationAttribute raitem in ra)
                    {
                        raitem.Owner = this;
                        raitem.OwnerProperty = pm;
                        Relations.Add(raitem.Partner, raitem);
                    }
                }
            }
            CreateSql();
        }
        private void CreateSql()
        {
            SelectDataReader sr = new SelectDataReader();
            bool isgroup = false; ;
            StringBuilder select = new StringBuilder();
            StringBuilder group = new StringBuilder();
            if (ID != null)
            {
                select.Append("(" + ID.ColumnName + ") as p_" + ID.Handler.Property.Name);
                group.Append(ID.ColumnName);
                sr.AddProperty(ID);
            }
            foreach (PropertyMapper pm in Properties)
            {
                sr.AddProperty(pm);
                if (select.Length > 0)
                {
                    select.Append(",");
                }
                
                if (pm.Aggregation != null)
                {
                    if (pm.Aggregation.DISTINCT)
                    {
                        select.Append("(" + pm.Aggregation.Type + "(DISTINCT " + pm.ColumnName + ")) as p_" + pm.Handler.Property.Name);
                    }
                    else
                    {
                        select.Append("(" + pm.Aggregation.Type + "(" + pm.ColumnName + ")) as p_" + pm.Handler.Property.Name);
                    }
                    isgroup = true;
                }
                else
                {
                    select.Append("(" + pm.ColumnName + ") as p_" + pm.Handler.Property.Name);
                    if (group.Length > 0)
                        group.Append(",");
                    group.Append(pm.ColumnName);
                }
                
            }
            sr.Select  ="Select "+ DISTINCT +  select.ToString()+" from {0}";
            
            mSelectReader.Add(ObjectType, sr);
            if (isgroup)
                sr.Group = group.ToString();
        }
        public Type ObjectType
        {
            get;
            set;
        }
        public SelectDataReader GetSelectReader(Type type)
        {
            lock (mSelectReader)
            {
                SelectDataReader reader;
                if (!mSelectReader.TryGetValue(type, out reader))
                {
                    reader = CreateSelectReader(type);
                    mSelectReader.Add(type, reader);
                }
                CurrentSelectReader = reader;
                return reader;
            }
        }
        private PropertyHandler MatchProperty(PropertyInfo pi, Type type)
        {
            
            PropertyInfo fpi = type.GetProperty(pi.Name, BindingFlags.Public| BindingFlags.Instance);
            if (fpi != null)
            {
                if (pi.PropertyType == fpi.PropertyType)
                {
                    return new PropertyHandler(fpi);
                }
            }
            return null;
        }
        public static void CheckMapper(ObjectMapper om, Type type)
        {
            if (om == null)
                throw new PeanutException(string.Format(DataMsg.OBJECT_MAPPING_NOTFOUND, type));
        }
        private SelectDataReader CreateSelectReader(Type type)
        {
            SelectDataReader sr = new SelectDataReader();
            if (type != ObjectType)
                sr.Proxy = true;
            bool isgroup = false; ;
            StringBuilder select = new StringBuilder();
            StringBuilder group = new StringBuilder();
            PropertyHandler handler=null;
            if (ID != null )
            {
                handler = MatchProperty(ID.Handler.Property, type);
                if (handler != null)
                {
                    select.Append("(" + ID.ColumnName + ") as p_" + ID.Handler.Property.Name);
                    group.Append(ID.ColumnName);
                    sr.AddProperty(ID);
                    sr.ProxyProperties.Add(handler);
                }
            }
            foreach (PropertyMapper pm in Properties)
            {
                handler = MatchProperty(pm.Handler.Property, type);
                if (handler == null)
                    continue;
                sr.ProxyProperties.Add(handler);
                sr.AddProperty(pm);
                if (select.Length > 0)
                {
                    select.Append(",");
                }

                if (pm.Aggregation != null)
                {
                    if (pm.Aggregation.DISTINCT)
                    {
                        select.Append("(" + pm.Aggregation.Type + "(DISTINCT " + pm.ColumnName + ")) as p_" + pm.Handler.Property.Name);
                    }
                    else
                    {
                        select.Append("(" + pm.Aggregation.Type + "(" + pm.ColumnName + ")) as p_" + pm.Handler.Property.Name);
                    }
                    isgroup = true;
                }
                else
                {
                    select.Append("(" + pm.ColumnName + ") as p_" + pm.Handler.Property.Name);
                    if (group.Length > 0)
                        group.Append(",");
                    group.Append(pm.ColumnName);
                }

            }
            sr.Select = "Select " + DISTINCT + select.ToString() + " from {0}";
            if (isgroup)
                sr.Group = group.ToString();
            if (sr.Properties.Count == 0)
                throw new PeanutException(string.Format(DataMsg.BOBJ_NOTMATCH_DOBJ, type, ObjectType));
            return sr;
        }
        public DObjectToBObject GetDTB(Type type)
        {
            lock (mDTBMap)
            {
                DObjectToBObject result = null;
                if (!mDTBMap.TryGetValue(type, out result))
                {
                    result = new DObjectToBObject(ObjectType, type);
                    mDTBMap.Add(type, result);
                }
                return result;
            }

        }
        public string GetSelectTable(SelectDataReader sr)
        {

            return string.Format(sr.Select,Table); 
        }
        public string DISTINCT
        {
            get;
            set;
        }
        [ThreadStatic]
        public static SelectDataReader CurrentSelectReader;

        [ThreadStatic]
        public static ObjectMapper CurrentOM;

        [ThreadStatic]
        private static string mCurrentTable=null;
        private string mTable;
        public void SetCurrentTable(string name)
        {
            mCurrentTable = name;
        }
        public void CleanCurrentTable()
        {
            mCurrentTable = null;
        }      
        public string Table
        {
            get
            {
                if (mCurrentTable != null)
                {
                    return mCurrentTable;
                }
                else
                {
                    return mTable;
                }
            }
        }
        public PropertyMapper ID
        {
            get;
            set;

        }
        public ConnectionAttribute Connection
        {
            get;
            set;

        }
        private IList<PropertyMapper> mProperties = new List<PropertyMapper>();
        public IList<PropertyMapper> Properties
        {
            get
            {
                return mProperties;
            }

        }
        private static Dictionary<Type, ObjectMapper> mMapperTable = new Dictionary<Type, ObjectMapper>();
        private static KeyValueCollection<ObjectMapper> mTableMapperTable = new KeyValueCollection<ObjectMapper>();
        public static ObjectMapper GetOM(string table)
        {
            CurrentOM= mTableMapperTable[table];
            return CurrentOM;
        }
        public static ObjectMapper GetOM(Type type)
        {
            ObjectMapper om;
                if (!mMapperTable.ContainsKey(type))
                {
                    lock (mMapperTable)
                    {
                        if (!mMapperTable.ContainsKey(type))
                        {
                             om = new ObjectMapper(type);
                            mMapperTable.Add(type,om);
                            mTableMapperTable.Add(om.Table, om);
                        }
                    }
                }
                om = mMapperTable[type];
                CurrentOM = om;
                return om;
           
        }
        private Dictionary<Type, RelationAttribute> mRelations = new Dictionary<Type, RelationAttribute>();
        public Dictionary<Type, RelationAttribute>Relations
        {
            get
            {
                return mRelations;
            }
        }
      
       
    }
}
