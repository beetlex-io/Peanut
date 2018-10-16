using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    [Serializable]
    public class DataObject : EntityBase
    {
        public DataObject() { }
        protected override void OnLoadData(System.Data.IDataReader reader)
        {
            ObjectMapper.CurrentSelectReader.ReaderToObject(reader, this);
        }

        public int Delete()
        {
            ObjectMapper om = ObjectMapper.GetOM(this.GetType());
            using (IConnectinContext cc = om.Connection.GetContext())
            {
                return Delete(cc);
            }
        }

        public int Delete(IConnectinContext cc)
        {
            int result = 0;
            ObjectMapper om = ObjectMapper.GetOM(this.GetType());
            if (om.ID == null)
                throw new PeanutException(DataMsg.ID_MAP_NOTFOUND);
            result = OnDeleteObject(cc, om.Table, om.ID.ColumnName,
                om.ID.Handler.Get(this));
            return result;
        }

        public int Save()
        {
            int result = 0;
            ObjectMapper om = ObjectMapper.GetOM(this.GetType());
            using (IConnectinContext cc = om.Connection.GetContext())
            {
                //cc.BeginTransaction();
                result = Save(cc);
                //cc.Commit();
            }
            return result;
        }

        public int Save(IConnectinContext cc)
        {
            int result = 0;
            ObjectMapper om = ObjectMapper.GetOM(this.GetType());

            if (EntityState._Loaded)
            {
                if (EntityState._FieldState.Count == 0)
                    return 0;
                result = EditData(cc, om);
            }
            else
            {
                if (EntityState._FieldState.Count == 0)
                    throw new PeanutException(DataMsg.OBJECT_UPDATA_NOTFOUND);
                result = NewData(cc, om);
                EntityState._Loaded = true;
            }
            EntityState._FieldState.Clear();
            return result;

        }

        public Peanut.Field[] GetChangeFields()
        {
            ObjectMapper om = ObjectMapper.GetOM(this.GetType());
            List<Field> fields = new List<Field>();
            Field[] result;
            for (int i = 0; i < om.Properties.Count; i++)
            {
                PropertyMapper pm = om.Properties[i];

                if (EntityState._FieldState.ContainsKey(pm.Handler.Property.Name))
                {


                    fields.Add(new Field
                    {
                        Name = pm.ColumnName,
                        ParameterName = pm.Handler.Property.Name,
                        Value = pm.Handler.Get(this),
                        IsParameter = true
                    });


                }
            }
            result = new Field[fields.Count];
            fields.CopyTo(result);
            return result;

        }

        private int EditData(IConnectinContext cc, ObjectMapper om)
        {
            if (om.ID == null)
                throw new PeanutException(DataMsg.UPDATE_ERROR_ID_NOTFOUND);
            Update update = new Update(om.Table);
            for (int i = 0; i < om.Properties.Count; i++)
            {
                PropertyMapper pm = om.Properties[i];

                System.Reflection.PropertyInfo pi = pm.Handler.Property;
                if (EntityState._FieldState.ContainsKey(pm.Handler.Property.Name))
                {
                    if (pm.Cast != null)
                    {
                        update.AddField(pm.ColumnName, "p_" + pi.Name, pm.Cast.ToColumn(pm.Handler.Get(this),
                            pm.Handler.Property.PropertyType, this));
                    }
                    else
                    {
                        update.AddField(pm.ColumnName, "p_" + pi.Name, pm.Handler.Get(this));
                    }
                }
            }
            update.Where.SqlText.Append(om.ID.ColumnName).Append("=@p1");
            update.Where.Parameters.Add(new Command.Parameter { Name = "p1", Value = om.ID.Handler.Get(this) });
            return update.Execute(cc);

        }

        private int NewData(IConnectinContext cc, ObjectMapper om)
        {
            int result = 0;
            Insert insert = new Insert(om.Table);
            object value = null;
            if (om.ID != null)
            {
                if (om.ID.Value != null)
                {
                    if (!om.ID.Value.AfterByUpdate)
                    {
                        om.ID.Value.Executing(cc, this, om.ID, om.Table);
                        insert.AddField(om.ID.ColumnName, om.ID.Handler.Get(this));
                    }
                }
                else
                {
                    insert.AddField(om.ID.ColumnName, om.ID.Handler.Get(this));
                }
            }
            for (int i = 0; i < om.Properties.Count; i++)
            {
                PropertyMapper pm = om.Properties[i];
                if (!EntityState._FieldState.ContainsKey(pm.Handler.Property.Name))
                {
                    if (pm.Value != null && !pm.Value.AfterByUpdate)
                    {
                        pm.Value.Executing(cc, this, pm, om.Table);
                    }
                }
                value = pm.Handler.Get(this);
                if (EntityState._FieldState.ContainsKey(pm.Handler.Property.Name))
                {
                    if (pm.Cast != null)
                    {

                        value = pm.Cast.ToColumn(value, pm.Handler.Property.PropertyType, this);
                    }
                    insert.AddField(pm.ColumnName, value);
                }
            }
            result = insert.Execute(cc);
            if (om.ID != null && om.ID.Value != null && om.ID.Value.AfterByUpdate)
                om.ID.Value.Executed(cc, this, om.ID, om.Table);
            return result;
        }

    }
}
