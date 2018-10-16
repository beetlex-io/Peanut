using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
namespace Peanut
{
    /// <summary>
    /// 实体基础对象
    /// </summary>
    [Serializable]
    public abstract class EntityBase : IEntityState
    {
        public EntityBase() { }
        private bool mLoadComplete = true;

        #region IEntityState 成员
        DB IEntityState.ConnectionType
        {
            get;
            set;
        }

        bool IEntityState._Loaded
        {
            get;
            set;
        }
        [NonSerialized]
        Dictionary<string, FieldState> m_FieldState = new Dictionary<string, FieldState>();

        Dictionary<string, FieldState> IEntityState._FieldState
        {
            get { return m_FieldState; }
        }

        void IEntityState.FieldChange(string field)
        {
            if (mLoadComplete)
                if (!m_FieldState.ContainsKey(field))
                {

                    m_FieldState.Add(field, new FieldState { ModifyTime = DateTime.Now });
                }
        }
      
        public IEntityState EntityState
        {
            get
            {
                return (IEntityState)this;
            }
        }

        #endregion

        void IEntityState.LoadData(IDataReader reader)
        {

            mLoadComplete = false;
            OnLoadData(reader);
            EntityState._Loaded = true;
            mLoadComplete = true;


        }

        protected abstract void OnLoadData(IDataReader reader);

        protected object Convert(Type type, object data)
        {
            return System.Convert.ChangeType(data, type);
        }

        private static IList OnList(Type type, IConnectinContext cc, string from, Expression expression, Region region, string orderby, string groupby)
        {
            Command cmd = Command.GetThreadCommand().AddSqlText(from);// new Command(from);
            if (expression != null)
                expression.Parse(cmd);

            if (!string.IsNullOrEmpty(groupby))
                cmd.Text.Append(" Group by ").Append(groupby);

            if (!string.IsNullOrEmpty(orderby))
                cmd.Text.Append(" Order by ").Append(orderby);
            return cc.List(type, cmd, region);
        }

        internal static IList ExOnList(Type type, IConnectinContext cc, string from, Expression expression, Region region, string orderby, string groupby)
        {
            return OnList(type, cc, from, expression, region, orderby, groupby);
        }

        private static object OnListFirst(Type type, IConnectinContext cc, string from, Expression expression, string orderby, string groupby)
        {
            Command cmd = Command.GetThreadCommand().AddSqlText(from);// new Command(from);
            if (expression != null)
                expression.Parse(cmd);

            if (!string.IsNullOrEmpty(groupby))
                cmd.Text.Append(" Group by ").Append(groupby);

            if (!string.IsNullOrEmpty(orderby))
                cmd.Text.Append(" Order by ").Append(orderby);
            return cc.ListFirst(type, cmd);
        }

        internal static object ExOnListFirst(Type type, IConnectinContext cc, string from, Expression expression, string orderby, string groupby)
        {
            return OnListFirst(type, cc, from, expression, orderby, groupby);
        }

        private static int OnCount(IConnectinContext cc, string table, Expression expression, string groupby)
        {
            Command cmd = Command.GetThreadCommand().AddSqlText("select count(*) from ").AddSqlText(table);// new Command("select count(*) from " + table);
            if (expression != null)
                expression.Parse(cmd);
            if (!string.IsNullOrEmpty(groupby))
                cmd.Text.Append(" Group by ").Append(groupby);
            object value = cc.ExecuteScalar(cmd);
            if (value == null || value == DBNull.Value)
                return 0;
            return System.Convert.ToInt32(value);

        }

        internal static int ExOnCount(IConnectinContext cc, string table, Expression expression, string groupby)
        {
            return OnCount(cc, table, expression, groupby);
        }

        private static int OnDelete(IConnectinContext cc, string table, Expression expression)
        {
            Delete del = new Delete(table);
            del.Where = expression;
            return del.Execute(cc);
        }

        internal static int ExOnDelete(IConnectinContext cc, string table, Expression expression)
        {
            return OnDelete(cc, table, expression);
        }

        private static T OnLoad<T>(IConnectinContext cc, string table, string id, object value) where T : IEntityState, new()
        {
            Command cmd = Command.GetThreadCommand().AddSqlText(table).AddSqlText(" where ").AddSqlText(id).AddSqlText("=@id");// new Command(table + " where " + id + "=@id");
            cmd.AddParameter("id", value);
            return cc.Load<T>(cmd);

        }

        internal static T ExOnLoad<T>(IConnectinContext cc, string table, string id, object value) where T : IEntityState, new()
        {
            return OnLoad<T>(cc, table, id, value);
        }

        protected int OnSave(IConnectinContext cc, string table, IList<Field> fields, Field id)
        {
            if (EntityState._FieldState.Count == 0)
                return 0;
            if (EntityState._Loaded)
            {
                return OnEdit(cc, table, fields, id);
            }
            else
            {
                return OnAdd(cc, table, fields, id);
            }
        }

        private int OnAdd(IConnectinContext cc, string table, IList<Field> fields, Field id)
        {
            int result = 0;
            Command cmd;
            Insert insert = new Insert(table);

            if (!string.IsNullOrEmpty(id.GetValueBy) && !id.GetValueAfterInsert)
            {
                cmd = Command.GetThreadCommand().AddSqlText(id.GetValueBy); //new Command(id.GetValueBy);
                id.Value = cc.ExecuteScalar(cmd);
                insert.AddField(id.Name, id.Value);
            }
            if (string.IsNullOrEmpty(id.GetValueBy))
            {
                insert.AddField(id.Name, id.Value);
            }
            for (int i = 0; i < fields.Count; i++)
            {
                Field f = fields[i];
                if (EntityState._FieldState.ContainsKey(f.Name))
                    insert.AddField(f.Name, f.Value);
            }
            result = insert.Execute(cc);
            if (!string.IsNullOrEmpty(id.GetValueBy) && id.GetValueAfterInsert)
            {
                cmd = Command.GetThreadCommand().AddSqlText(id.GetValueBy);// new Command(id.GetValueBy);
                id.Value = cc.ExecuteScalar(cmd);
            }
            EntityState._Loaded = true;
            return result;
        }

        private int OnEdit(IConnectinContext cc, string table, IList<Field> fields, Field id)
        {
            Update update = new Update(table);
            for (int i = 0; i < fields.Count; i++)
            {
                Field f = fields[i];

                if (EntityState._FieldState.ContainsKey(f.Name))
                    update.AddField(f.Name, f.ParameterName, f.Value);
            }
            update.Where.SqlText.Append(id.Name).Append("=@p1");
            update.Where.Parameters.Add(new Command.Parameter { Name = "p1", Value = id.Value });
            return update.Execute(cc);
        }

        protected int OnDeleteObject(IConnectinContext cc, string table, string id, object value)
        {
            Delete del = new Delete(table);
            del.Where.SqlText.Append(id).Append("=@p1");
            del.Where.Parameters.Add(new Command.Parameter { Name = "p1", Value = value });
            return del.Execute(cc);
        }

        private static object OnAggregation(IConnectinContext cc, string table, string aggregation, string field, bool DISTINCT, Expression expression, string groupby)
        {
            Command cmd = Command.GetThreadCommand();
            cmd.Text.Append(" select ").Append(aggregation).Append(" (").Append(DISTINCT ? "DISTINCT" : "").Append(field).Append(") from ").Append(table);
            if (!string.IsNullOrEmpty(groupby))
                cmd.Text.Append(" Group by ").Append(groupby);
            if (expression != null)
                expression.Parse(cmd);
            return cc.ExecuteScalar(cmd);


        }

        internal static object ExOnAggregation(IConnectinContext cc, string table, string aggregation, string field, bool DISTINCT, Expression expression, string groupby)
        {
            return OnAggregation(cc, table, aggregation, field, DISTINCT, expression, groupby);
        }







    }
}
