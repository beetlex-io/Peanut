using System;
using System.Collections.Generic;
using System.Text;
using Peanut.Mappings;
using System.Collections;

namespace Peanut
{
    /// <summary>
    /// 条件表达式扩展操作
    /// </summary>
    public partial class Expression
    {
        /// <summary>
        /// 获取指定类型的对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns></returns>
        public IList<T> List<T>() where T : Mappings.DataObject, new()
        {
            return List<T>((Region)null);
        }
        /// <summary>
        /// 获取指定类型的对象列表,并指定排序规则
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="orderby">排序规则</param>
        /// <returns></returns>
        public IList<T> List<T>(params string[] orderby) where T : Mappings.DataObject, new()
        {
            return List<T>(null as Region, orderby);
        }
        /// <summary>
        /// 获取指定区间的对象列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="region">区间</param>
        /// <returns></returns>
        public IList<T> List<T>(Region region) where T : Mappings.DataObject, new()
        {
            return List<T>(region, null);
        }
        public IList<T> List<T>(Region region, params string[] orderby) where T : Mappings.DataObject, new()
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            using (IConnectinContext cc = om.Connection.GetContext())
            {
                return List<T>(cc, region, orderby);
            }
        }
        public IList<T> List<T>(DB type, params string[] orderby) where T : Mappings.DataObject, new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return List<T>(cc, null, orderby);
            }
        }
        public IList<T> List<T>(IConnectinContext cc, params string[] orderby) where T : Mappings.DataObject, new()
        {
            return List<T>(cc, null, orderby);
        }
        public IList<T> List<T>(DB type, Region region) where T : Mappings.DataObject, new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return List<T>(cc, region, null);
            }
        }
        public IList<T> List<T>(IConnectinContext cc, Region region) where T : Mappings.DataObject, new()
        {
            return List<T>(cc, region, null);
        }
        public IList<T> List<T>(DB ct, Region region, params string[] orderby) where T : Mappings.DataObject, new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(ct))
            {
                Type type = typeof(T);
                return (IList<T>)List(type, cc, region, orderby);
            }
        }
        public IList<T> List<T>(IConnectinContext cc, Region region, params string[] orderby) where T : Mappings.DataObject, new()
        {
            Type type = typeof(T);
            return (IList<T>)List(type, cc, region, orderby);
        }
       
        public IList List(Type type, IConnectinContext cc, Region region, params string[] orderby)
        {
            ObjectMapper om = ObjectMapper.GetOM(type);

            SelectDataReader sr = om.GetSelectReader(type);
            string strob = null;
            if (orderby != null && orderby.Length > 0)
                strob = string.Join(",", orderby);
            return EntityBase.ExOnList(type, cc, om.GetSelectTable(sr), this, region, strob, sr.Group);
        }
        public IList<RESULT> List<T, RESULT>()
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            return List<T, RESULT>((Region)null);
        }
        public IList<RESULT> List<T, RESULT>(params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            return List<T, RESULT>((Region)null, orderby);
        }
        public IList<RESULT> List<T, RESULT>(Region region)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            return List<T, RESULT>(region, null);
        }
        public IList<RESULT> List<T, RESULT>(Region region, params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return List<T, RESULT>(cc, region, orderby);
            }
        }
        public IList<RESULT> List<T, RESULT>(DB type, Region region)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return List<T, RESULT>(cc, region, null);
            }
        }
        public IList<RESULT> List<T, RESULT>(IConnectinContext cc, Region region)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            return List<T, RESULT>(cc, region, null);
        }
        public IList<RESULT> List<T, RESULT>(DB type, params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return List<T, RESULT>(cc, null, orderby);
            }
        }
        public IList<RESULT> List<T, RESULT>(IConnectinContext cc, params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            return List<T, RESULT>(cc, null, orderby);
        }
        public IList<RESULT> List<T, RESULT>(DB type, Region region, params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return List<T, RESULT>(cc, region, orderby);
            }
        }
        public IList<RESULT> List<T, RESULT>(IConnectinContext cc, Region region, params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {

            Type type = typeof(RESULT);
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            SelectDataReader sr = om.GetSelectReader(type);
            string strob = null;
            if (orderby != null && orderby.Length > 0)
                strob = string.Join(",", orderby);
            return (IList<RESULT>)EntityBase.ExOnList(typeof(RESULT), cc, om.GetSelectTable(sr), this, region, strob, sr.Group);

        }


        public RESULT ListFirst<T, RESULT>()
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            return ListFirst<T, RESULT>((string)null);
        }
        public RESULT ListFirst<T, RESULT>(DB type)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return ListFirst<T, RESULT>(cc);
            }
        }
        public RESULT ListFirst<T, RESULT>(IConnectinContext cc)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            return ListFirst<T, RESULT>(cc, null);
        }
        public RESULT ListFirst<T, RESULT>(params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {

            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            using (IConnectinContext cc = om.Connection.GetContext())
            {
                return ListFirst<T, RESULT>(cc, orderby);
            }

        }
        public RESULT ListFirst<T, RESULT>(DB type, params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return ListFirst<T, RESULT>(cc, orderby);
            }
        }
        public RESULT ListFirst<T, RESULT>(IConnectinContext cc, params string[] orderby)
            where T : Mappings.DataObject, new()
            where RESULT : new()
        {
            Type type = typeof(T);
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            SelectDataReader sr = om.GetSelectReader(typeof(RESULT));
            string strob = null;
            if (orderby != null && orderby.Length > 0)
                strob = string.Join(",", orderby);
            return (RESULT)EntityBase.ExOnListFirst(typeof(RESULT), cc, om.GetSelectTable(sr), this, strob, sr.Group);
        }
        public T ListFirst<T>() where T : Mappings.DataObject, new()
        {
            return ListFirst<T>((string)null);
        }
        public T ListFirst<T>(DB type) where T : Mappings.DataObject, new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return ListFirst<T>(cc, null);
            }
        }
        public T ListFirst<T>(IConnectinContext cc) where T : Mappings.DataObject, new()
        {
            return ListFirst<T>(cc, null);
        }
        public T ListFirst<T>(params string[] orderby) where T : Mappings.DataObject, new()
        {

            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            using (IConnectinContext cc = om.Connection.GetContext())
            {
                return ListFirst<T>(cc, orderby);
            }

        }
        public object ListFirst(Type type, IConnectinContext cc, params string[] orderby)
        {
            ObjectMapper om = ObjectMapper.GetOM(type);
            SelectDataReader sr = om.GetSelectReader(type);
            string strob = null;
            if (orderby != null && orderby.Length > 0)
                strob = string.Join(",", orderby);
            return EntityBase.ExOnListFirst(type, cc, om.GetSelectTable(sr), this, strob, sr.Group);
        }
        public T ListFirst<T>(DB type, params string[] orderby) where T : Mappings.DataObject, new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return ListFirst<T>(cc, orderby);
            }
        }
        public T ListFirst<T>(IConnectinContext cc, params string[] orderby) where T : Mappings.DataObject, new()
        {
            Type type = typeof(T);
            return (T)ListFirst(typeof(T), cc, orderby);
        }

        public int Delete<T>() where T : Mappings.DataObject
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            using (IConnectinContext cc = om.Connection.GetContext())
            {
                return Delete<T>(cc);
            }
        }
        public int Delete<T>(DB type) where T : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Delete<T>(cc);
            }
        }
        public int Delete<T>(IConnectinContext cc) where T : Mappings.DataObject
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            return EntityBase.ExOnDelete(cc, om.Table, this);
        }

        public int Edit<T>(Action<T> handler) where T : DataObject, new()
        {
            T item = new T();
            handler(item);
            return Edit<T>(item.GetChangeFields());
        }
        public int Edit<T>(DB type, Action<T> handler) where T : DataObject, new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Edit<T>(cc, handler);
            }
        }
        public int Edit<T>(IConnectinContext cc, Action<T> handler) where T : DataObject, new()
        {
            T item = new T();
            handler(item);
            return Edit<T>(cc, item.GetChangeFields());
        }
        public int Edit<T>(params Field[] fields) where T : DataObject, new()
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            using (IConnectinContext cc = om.Connection.GetContext())
            {
                return Edit<T>(cc, fields);
            }
        }
        public int Edit<T>(DB type, params Field[] fields) where T : DataObject, new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Edit<T>(cc, fields);
            }
        }
        public int Edit<T>(IConnectinContext cc, params Field[] fields) where T : DataObject, new()
        {
            if (fields == null || fields.Length == 0)
                return 0;
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            Update update = new Update(om.Table);
            update.Where = this;
            for (int i = 0; i < fields.Length; i++)
            {
                Field f = fields[i];
                update.AddField(f.Name, f.ParameterName, Mappings.PropertyCastAttribute.CastValue(om, f.Name, f.Value));
            }
            return update.Execute(cc);
        }

        public int Count<T>() where T : Mappings.DataObject
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));

            using (IConnectinContext cc = om.Connection.GetContext())
            {
                return Count<T>(cc);
            }
        }
        public int Count<T>(DB type) where T : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Count<T>(cc);
            }
        }
        public int Count<T>(IConnectinContext cc) where T : Mappings.DataObject
        {
            Type type = typeof(T);
            ObjectMapper om = ObjectMapper.GetOM(type);
            SelectDataReader sr = om.GetSelectReader(type);
            return EntityBase.ExOnCount(cc, om.Table, this, sr.Group);
        }
        public int Count<T>(string field) where T : Mappings.DataObject
        {
            return Count<T>(field, false);
        }
        public int Count<T>(string field, bool DISTINCT) where T : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return Count<T>(field, DISTINCT, cc);
            }
        }
        public int Count<T>(string field, DB type) where T : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Count<T>(field, false, cc);
            }
        }
        public int Count<T>(string field, IConnectinContext cc) where T : Mappings.DataObject
        {
            return Count<T>(field, false, cc);
        }
        public int Count<T>(string field, bool DISTINCT, DB type) where T : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Count<T>(field, DISTINCT, cc);
            }
        }
        public int Count<T>(string field, bool DISTINCT, IConnectinContext cc) where T : Mappings.DataObject
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(T));
            object value = EntityBase.ExOnAggregation(cc, om.Table, "count", field, DISTINCT, this, null);
            if (value == null || value == DBNull.Value)
                return 0;
            return (int)Convert.ChangeType(value, typeof(int));
        }

        public RESULT Sum<RESULT, Entity>(string field) where Entity : Mappings.DataObject
        {
            return Sum<RESULT, Entity>(field, false);
        }
        public RESULT Sum<RESULT, Entity>(string field, bool DISTINCT) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return Sum<RESULT, Entity>(field, DISTINCT, cc);
            }
        }
        public RESULT Sum<RESULT, Entity>(string field, DB type) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Sum<RESULT, Entity>(field, false, cc);
            }
        }
        public RESULT Sum<RESULT, Entity>(string field, IConnectinContext cc) where Entity : Mappings.DataObject
        {
            return Sum<RESULT, Entity>(field, false, cc);
        }
        public RESULT Sum<RESULT, Entity>(string field, bool DISTINCT, DB type) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Sum<RESULT, Entity>(field, DISTINCT, cc);
            }
        }
        public RESULT Sum<RESULT, Entity>(string field, bool DISTINCT, IConnectinContext cc) where Entity : Mappings.DataObject
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(Entity));
            object value = EntityBase.ExOnAggregation(cc, om.Table, "Sum", field, DISTINCT, this, null);
            if (value == null || value == DBNull.Value)
                return default(RESULT);
            return (RESULT)Convert.ChangeType(value, typeof(RESULT));
        }

        public RESULT Max<RESULT, Entity>(string field) where Entity : Mappings.DataObject
        {
            return Max<RESULT, Entity>(field, false);
        }
        public RESULT Max<RESULT, Entity>(string field, bool DISTINCT) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return Max<RESULT, Entity>(field, DISTINCT, cc);
            }
        }
        public RESULT Max<RESULT, Entity>(string field, DB type) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Max<RESULT, Entity>(field, false, cc);
            }
        }
        public RESULT Max<RESULT, Entity>(string field, IConnectinContext cc) where Entity : Mappings.DataObject
        {
            return Max<RESULT, Entity>(field, false, cc);
        }
        public RESULT Max<RESULT, Entity>(string field, bool DISTINCT, DB type) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Max<RESULT, Entity>(field, DISTINCT, cc);
            }
        }
        public RESULT Max<RESULT, Entity>(string field, bool DISTINCT, IConnectinContext cc) where Entity : Mappings.DataObject
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(Entity));
            object value = EntityBase.ExOnAggregation(cc, om.Table, "Max", field, DISTINCT, this, null);
            if (value == null || value == DBNull.Value)
                return default(RESULT);
            return (RESULT)Convert.ChangeType(value, typeof(RESULT));
        }

        public RESULT Min<RESULT, Entity>(string field) where Entity : Mappings.DataObject
        {
            return Min<RESULT, Entity>(field, false);
        }
        public RESULT Min<RESULT, Entity>(string field, bool DISTINCT) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return Min<RESULT, Entity>(field, DISTINCT, cc);
            }
        }
        public RESULT Min<RESULT, Entity>(string field, DB type) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Min<RESULT, Entity>(field, false, cc);
            }
        }
        public RESULT Min<RESULT, Entity>(string field, IConnectinContext cc) where Entity : Mappings.DataObject
        {
            return Min<RESULT, Entity>(field, false, cc);
        }
        public RESULT Min<RESULT, Entity>(string field, bool DISTINCT, DB type) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Min<RESULT, Entity>(field, DISTINCT, cc);
            }
        }
        public RESULT Min<RESULT, Entity>(string field, bool DISTINCT, IConnectinContext cc) where Entity : Mappings.DataObject
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(Entity));
            object value = EntityBase.ExOnAggregation(cc, om.Table, "Min", field, DISTINCT, this, null);
            if (value == null || value == DBNull.Value)
                return default(RESULT);
            return (RESULT)Convert.ChangeType(value, typeof(RESULT));
        }

        public RESULT Avg<RESULT, Entity>(string field) where Entity : Mappings.DataObject
        {
            return Avg<RESULT, Entity>(field, false);
        }
        public RESULT Avg<RESULT, Entity>(string field, bool DISTINCT) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return Avg<RESULT, Entity>(field, DISTINCT, cc);
            }
        }
        public RESULT Avg<RESULT, Entity>(string field, DB type) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Avg<RESULT, Entity>(field, false, cc);
            }
        }
        public RESULT Avg<RESULT, Entity>(string field, IConnectinContext cc) where Entity : Mappings.DataObject
        {
            return Avg<RESULT, Entity>(field, false, cc);
        }
        public RESULT Avg<RESULT, Entity>(string field, bool DISTINCT, DB type) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Avg<RESULT, Entity>(field, DISTINCT, cc);
            }
        }
        public RESULT Avg<RESULT, Entity>(string field, bool DISTINCT, IConnectinContext cc) where Entity : Mappings.DataObject
        {
            ObjectMapper om = ObjectMapper.GetOM(typeof(Entity));
            object value = EntityBase.ExOnAggregation(cc, om.Table, "Avg", field, DISTINCT, this, null);
            if (value == null || value == DBNull.Value)
                return default(RESULT);
            return (RESULT)Convert.ChangeType(value, typeof(RESULT));
        }
        #region GetValue
        public RESULT GetValue<RESULT, Entity>(IFieldInfo field) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return GetValue<RESULT, Entity>(field);
            }
        }
        public RESULT GetValue<RESULT, Entity>(IFieldInfo field, DB type, params string[] orderby) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return GetValue<RESULT, Entity>(field, cc, orderby);
            }
        }
        public RESULT GetValue<RESULT, Entity>(IFieldInfo field, IConnectinContext cc, params string[] orderby) where Entity : Mappings.DataObject
        {
            Command cmd = Command.GetThreadCommand();
            ObjectMapper om = ObjectMapper.GetOM(typeof(Entity));
            cmd.Text.AppendFormat("select {0} from {1} ", field.Name, om.Table);
            this.Parse(cmd);
            if (orderby != null && orderby.Length > 0)
                cmd.Text.Append(" Order by ").Append(string.Join(",", orderby));
            return cc.GetValue<RESULT>(cmd);

        }

        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return GetValues<RESULT, Entity>(field, cc, (Region)null);
            }
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, params string[] orderby) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return GetValues<RESULT, Entity>(field, cc, (Region)null, orderby);
            }
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, int pageindex, int pagesize, params string[] orderby) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return GetValues<RESULT, Entity>(field, cc, new Region(pageindex, pagesize), orderby);
            }
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, int pageindex, int pagesize) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return GetValues<RESULT, Entity>(field, cc, new Region(pageindex, pagesize));
            }
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, DB type, int pageindex, int pagesize) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return GetValues<RESULT, Entity>(field, cc, pageindex, pagesize);
            }
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, IConnectinContext cc, int pageindex, int pagesize) where Entity : Mappings.DataObject
        {
            return GetValues<RESULT, Entity>(field, cc, new Region(pageindex, pagesize));
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, Region region) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return GetValues<RESULT, Entity>(field, cc, region);
            }
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, Region region, params string[] orderby) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return GetValues<RESULT, Entity>(field, cc, region, orderby);
            }
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, DB type, Region region, params string[] orderby) where Entity : Mappings.DataObject
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return GetValues<RESULT, Entity>(field, cc, region, orderby);
            }
        }
        public IList<RESULT> GetValues<RESULT, Entity>(IFieldInfo field, IConnectinContext cc, Region region, params string[] orderby) where Entity : Mappings.DataObject
        {
            Command cmd = Command.GetThreadCommand();
            ObjectMapper om = ObjectMapper.GetOM(typeof(Entity));
            if (DBContext.GetRegionValues == null || !DBContext.GetRegionValues(field, om.Table, cc, region, this, orderby))
            {
                cmd.Text.AppendFormat("select {0} from {1} ", field.Name, om.Table);
                this.Parse(cmd);
                if (orderby != null && orderby.Length > 0)
                    cmd.Text.Append(" Order by ").Append(string.Join(",", orderby));
            }
            return cc.GetValues<RESULT>(cmd, region);
        }
        #endregion

        #region join table

        public IList<T> List<T>(JoinTable table, IConnectinContext cc) where T : new()
        {
            return List<T>(table, cc, null, null);
        }
        public IList<T> List<T>(JoinTable table, Region region) where T : new()
        {
            return List<T>(table, region, null);
        }
        public IList<T> List<T>(JoinTable table, Region region, params string[] orders) where T : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(DB.DB1))
            {
                return List<T>(table, cc, region, orders);
            }
        }
        public IList<T> List<T>(JoinTable table, IConnectinContext cc,Region region,params string[] orders) where T : new()
        {
         
            SQL sql = new SQL(table.ToString());
            if (this.SqlText.Length > 0)
            {
                sql.AddSql(" where " + this.ToString());
                foreach (Command.Parameter p in this.Parameters)
                {
                    sql.Parameter(p.Name, p.Value);
                }
            }
            if (orders != null && orders.Length > 0)
            {
                sql.AddSql(" order by " + string.Join(",", orders));
            }
            IList<T> result= sql.List<T>(cc, region);
          
            return result;
        }

        public IList<T> List<T>(JoinTable table) where T:new()
        {
            return List<T>(table, (string)null);
        }
        public IList<T> List<T>(JoinTable table, params string[] orders) where T : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(DB.DB1))
            {
                return List<T>(table, cc, null, null);
            }
        }
        #endregion

    }
    public delegate bool EventGetRegionValues(IFieldInfo field, string table, IConnectinContext cc, Region region, Expression exp, string[] orderby);
}
