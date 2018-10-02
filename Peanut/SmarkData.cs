using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections;

// 开源协议:Apache License, Version 2.0
// Copyright © FanJianHan 2008
// mail:henryfan@msn.com
namespace Peanut
{






    /// <summary>

    /// MSSQL数据库
    /// </summary>
    public class MSSQL : IDriver
    {
        #region IDriver 成员

        public IDbConnection Connection
        {
            get { return new SqlConnection(); }
        }

        public IDbDataAdapter DataAdapter(IDbCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = (SqlCommand)cmd;
            return da;
        }

        public IDbCommand Command
        {
            get
            {
                return new SqlCommand();
            }
        }

        public string ReplaceSql(String sql)
        {
            return sql.Replace("@", "@");
        }

        public IDataParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            SqlParameter sp = new SqlParameter("@" + name, value);
            if (value == null)
                sp.Value = DBNull.Value;
            sp.Direction = direction;
            return sp;
        }

        #endregion


        public IDataParameter CreateProcParameter(string name, object value, ParameterDirection direction)
        {
            SqlParameter sp = new SqlParameter("@" + name, value);
            if (value == null)
                sp.Value = DBNull.Value;
            sp.Direction = direction;
            return sp;
        }
    }
    ///// <summary>

    ///// ACCESS数据库
    ///// </summary>
    //public class MSAccess : IDriver
    //{
    //    #region IDriver 成员

    //    public IDbConnection Connection
    //    {
    //        get { return new OleDbConnection(); }
    //    }

    //    public IDbDataAdapter DataAdapter(IDbCommand cmd)
    //    {
    //        OleDbDataAdapter da = new OleDbDataAdapter();
    //        da.SelectCommand = (OleDbCommand)cmd;
    //        return da;
    //    }

    //    public IDbCommand Command
    //    {
    //        get
    //        {
    //            return new OleDbCommand();
    //        }
    //    }

    //    public string ReplaceSql(String sql)
    //    {
    //        return sql.Replace("@", "@");
    //    }

    //    public IDataParameter CreateParameter(string name, object value, ParameterDirection direction)
    //    {
    //        OleDbParameter sp = new OleDbParameter("@" + name, value);
    //        sp.Direction = direction;
    //        if (value == null)
    //            sp.Value = DBNull.Value;
    //        else if (value.GetType() == typeof(DateTime))
    //        {
    //            sp.Value = value.ToString();
    //        }
    //        else
    //        {
    //            sp.Value = value;
    //        }
    //        return sp;
    //    }

    //    #endregion


    //    public IDataParameter CreateProcParameter(string name, object value, ParameterDirection direction)
    //    {
    //        OleDbParameter sp = new OleDbParameter("@" + name, value);
    //        sp.Direction = direction;
    //        if (value == null)
    //            sp.Value = DBNull.Value;
    //        else if (value.GetType() == typeof(DateTime))
    //        {
    //            sp.Value = value.ToString();
    //        }
    //        else
    //        {
    //            sp.Value = value;
    //        }
    //        return sp;
    //    }
    //}

    /// <summary> 
    /// 命令执行描述
    /// </summary>
    public interface ICommandExecute
    {
        int Execute(IConnectinContext cc);
    }
    /// <summary>

    /// 添加数据命令
    /// </summary>
    public class Insert : ICommandExecute
    {


        public Insert(string table)
        {

            mTable = table;

        }
        private string mTable;
        private IList<Field> mInserFields = new List<Field>();
        public Insert AddField(string name, object value)
        {
            AddField(name, value, true);
            return this;
        }
        public Insert AddField(string name, object value, bool isparameter)
        {
            Field f = new Field();
            f.IsParameter = isparameter;
            f.Name = name;
            f.Value = value;
            mInserFields.Add(f);
            return this;
        }
        public int Execute(IConnectinContext cc)
        {
            Command mCommand = Command.GetThreadCommand().AddSqlText("Insert into ").AddSqlText(mTable);
            System.Text.StringBuilder names, values;
            names = new System.Text.StringBuilder();
            values = new System.Text.StringBuilder();
            Field field;
            for (int i = 0; i < mInserFields.Count; i++)
            {
                if (i > 0)
                {
                    names.Append(",");
                    values.Append(",");
                }
                field = (Field)mInserFields[i];

                names.Append(field.Name);
                if (field.IsParameter)
                {
                    values.Append("@").Append(field.ParameterName);
                    if (field.Value != null)
                        mCommand.AddParameter(field.ParameterName, field.Value);
                    else
                        mCommand.AddParameter(field.ParameterName, DBNull.Value);
                }
                else
                    values.Append(field.Value);

            }
            mCommand.Text.Append("(").Append(names).Append(")").Append(" Values(").Append(values).Append(")");
            return cc.ExecuteNonQuery(mCommand);


        }
    }
    /// <summary>
    /// 字段值描述
    /// </summary>
    public class Field
    {
        private object mValue;
        public object Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
            }
        }
        private string mParameterName;
        public string ParameterName
        {
            get
            {
                return string.IsNullOrEmpty(mParameterName) ? Name : mParameterName;
            }
            set
            {
                mParameterName = value;
            }
        }
        private string mName;
        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }
        private bool mIsParameter = true;
        public bool IsParameter
        {
            get
            {
                return mIsParameter;
            }
            set
            {
                mIsParameter = value;
            }
        }
        public string GetValueBy
        {
            get;
            set;
        }
        public bool GetValueAfterInsert
        {
            get;
            set;
        }

    }
    /// <summary>
    /// SQL执行映射对象
    /// </summary>
    public class SQL
    {
        /// <summary>
        /// 构建SQL对象并指定相应的sql语句
        /// </summary>
        /// <param name="sql"></param>
        public SQL(string sql)
        {
            mBaseSql = sql;
            mCommand.AddSqlText(sql);
        }
        public static SQL operator +(string subsql, SQL sql)
        {
            sql.AddSql(subsql);
            return sql;
        }
        public static SQL operator +(SQL sql, string subsql)
        {
            sql.AddSql(subsql);
            return sql;
        }
        /// <summary>
        /// 设置SQL参数值
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>SQL对象</returns>
        public SQL this[string name, object value]
        {
            get
            {
                return Parameter(name, value);
            }
        }

        public static implicit operator SQL(string sql)
        {
            return new SQL(sql);
        }

        private string mBaseSql;

        private Command mCommand = new Command("");

        public Command Command
        {
            get
            {
                return mCommand;
            }
        }

        public SQL AddSql(string sql)
        {
            mCommand.AddSqlText(sql);
            return this;
        }
        public SQL Parameter(string name, object value)
        {
            mCommand.AddParameter(name, value);
            return this;
        }
        /// <summary>
        /// 执行SQL并返回受影响的数量
        /// </summary>
        /// <returns>int</returns>
        public int Execute()
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return Execute(cc);
            }
        }
        /// <summary>
        /// 在指写数据库上执行SQL并返回受影响的数量
        /// </summary>
        /// <param name="type">数据库配置项</param>
        /// <returns>int</returns>
        public int Execute(DB type)
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return Execute(cc);
            }
        }
        /// <summary>
        /// 在指写数据库上执行SQL并返回受影响的数量
        /// </summary>
        /// <param name="cc">数据库访问上下文</param>
        /// <returns>int</returns>
        public int Execute(IConnectinContext cc)
        {
            return cc.ExecuteNonQuery(mCommand);
        }
        /// <summary>
        /// 执行SQL并返回指定类型的值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <returns>返回值</returns>
        public T GetValue<T>()
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return GetValue<T>(cc);
            }
        }
        /// <summary>
        /// 在指写数据库上执行SQL并返回指定类型的值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="type">数据库配置项</param>
        /// <returns>返回值</returns>
        public T GetValue<T>(DB type)
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return GetValue<T>(cc);
            }
        }


        /// <summary>
        /// 在指写数据库上执行SQL并返回指定类型的值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="cc">数据库访问上下文</param>
        /// <returns>返回值</returns>
        public T GetValue<T>(IConnectinContext cc)
        {
            return (T)cc.ExecuteScalar(GetCommand());
        }
        /// <summary>
        /// 执行SQL返回第一条记录
        /// </summary>
        /// <typeparam name="T">记录对象类型</typeparam>
        /// <returns>返回对象</returns>
        public T ListFirst<T>() where T : new()
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return ListFirst<T>(cc);
            }
        }
        /// <summary>
        /// 在指定的数据库上执行SQL返回第一条记录
        /// </summary>
        /// <typeparam name="T">记录对象类型</typeparam>
        /// <param name="type">数据库配置项</param>
        /// <returns>返回对象</returns>
        public T ListFirst<T>(DB type) where T : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return ListFirst<T>(cc);
            }
        }
        /// <summary>
        /// 在指定的数据库上执行SQL返回第一条记录
        /// </summary>
        /// <typeparam name="T">记录对象类型</typeparam>
        /// <param name="cc">数据库访问上下文</param>
        /// <returns>返回对象</returns>
        public T ListFirst<T>(IConnectinContext cc) where T : new()
        {
            IList<T> result = List<T>(cc, new Region(0, 2));
            if (result.Count > 0)
                return result[0];
            return default(T);

        }

        internal object ListFirst(Type type, IConnectinContext cc)
        {
            IList result = List(type, cc, null);
            if (result.Count > 0)
                return result[0];
            return null;
        }
        /// <summary>
        /// 执行SQL并返回记录列表
        /// </summary>
        /// <typeparam name="T">记录类型</typeparam>
        /// <returns>对象列表</returns>
        public IList<T> List<T>() where T : new()
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return List<T>(cc);
            }
        }
        /// <summary>
        ///  执行SQL并返回指定区间记录列表
        /// </summary>
        /// <typeparam name="T">记录类型</typeparam>
        /// <param name="region">区间</param>
        /// <returns>对象列表</returns>
        public IList<T> List<T>(Region region) where T : new()
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return List<T>(cc, region);
            }
        }
        /// <summary>
        /// 在指定数据库上执行SQL并返回记录列表
        /// </summary>
        /// <typeparam name="T">记录类型</typeparam>
        /// <param name="type">数据配置项</param>
        /// <returns>对象列表</returns>
        public IList<T> List<T>(DB type) where T : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return List<T>(cc);
            }
        }
        /// <summary>
        /// 在指定数据库上执行SQL并返回记录列表
        /// </summary>
        /// <typeparam name="T">记录类型</typeparam>
        /// <param name="cc">数据库访问上下文</param>
        /// <returns>对象列表</returns>
        public IList<T> List<T>(IConnectinContext cc) where T : new()
        {
            return List<T>(cc, null);
        }

        internal IList List(Type type, IConnectinContext cc, Region region)
        {
           
            System.Type itemstype = System.Type.GetType("System.Collections.Generic.List`1");
            itemstype = itemstype.MakeGenericType(type);
            IList result;
            if (region == null)
            {
                region = new Region(0, 99999999);
            }
            if (region.Size > DBContext.DefaultListMaxSize)
                result = (IList)Activator.CreateInstance(itemstype, DBContext.DefaultListMaxSize);
            else
                result = (IList)Activator.CreateInstance(itemstype, region.Size);
            Mappings.CommandReader cr = Mappings.CommandReader.GetReader(mBaseSql, type);
            int index = 0;
            Command cmd = GetCommand();

            using (IDataReader reader = cc.ExecuteReader(cmd))
            {

                while (reader.Read())
                {
                    if (index >= region.Start)
                    {
                        object item = Activator.CreateInstance(type);
                        cr.ReaderToObject(reader, item);
                        result.Add(item);
                        if (result.Count >= region.Size)
                        {
                            cmd.DbCommand.Cancel();
                            reader.Dispose();
                            break;
                        }
                    }
                    index++;
                }

            }

            return result;

        }

        /// <summary>
        /// 在指定数据库上执行SQL并返回指定区间记录列表
        /// </summary>
        /// <typeparam name="T">记录类型</typeparam>
        /// <param name="type">数据配置项</param>
        /// <param name="region">区间</param>
        /// <returns>对象列表</returns>
        public IList<T> List<T>(DB type, Region region) where T : new()
        {
            using (IConnectinContext cc = DBContext.GetConnection(type))
            {
                return List<T>(cc, region);
            }
        }
        /// <summary>
        /// 在指定数据库上执行SQL并返回指定区间记录列表
        /// </summary>
        /// <typeparam name="T">记录类型</typeparam>
        /// <param name="cc">数据库访问上下文</param>
        /// <param name="region">区间</param>
        /// <returns>对象列表</returns>
        public IList<T> List<T>(IConnectinContext cc, Region region) where T : new()
        {
            return (IList<T>)List(typeof(T), cc, region);

        }

        private Command GetCommand()
        {

            return mCommand;
        }
    }

    /// <summary>
    /// 更新数据命令
    /// </summary>
    public class Update : ICommandExecute
    {

        public Update(string table)
        {

            mTable = table;
        }
        private string mTable;
        private IList<Field> mFields = new List<Field>();
        public Update AddField(string name, string parametername, object value)
        {
            AddField(name, parametername, value, true);
            return this;
        }
        public Update AddField(string name, string parametername, object value, bool isparameter)
        {
            Field f = new Field();
            f.IsParameter = isparameter;
            f.Name = name;
            f.ParameterName = parametername;
            f.Value = value;
            mFields.Add(f);
            return this;
        }
        private Expression mWhere = new Expression();
        public Expression Where
        {
            get
            {
                return mWhere;
            }
            set
            {
                mWhere = value;
            }
        }
        #region ICommandExecute 成员
        public int Execute()
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return Execute(cc);
            }
        }
        public int Execute(IConnectinContext cc)
        {
            Command mCommand = Command.GetThreadCommand().AddSqlText("Update ").AddSqlText(mTable).AddSqlText(" set ");
            for (int i = 0; i < mFields.Count; i++)
            {
                if (i > 0)
                    mCommand.Text.Append(",");
                if (!mFields[i].IsParameter)
                {
                    mCommand.Text.Append(mFields[i].Name).Append("=").Append(mFields[i].Value);
                }
                else
                {
                    mCommand.Text.Append(mFields[i].Name).Append("=@").Append(mFields[i].ParameterName);
                    mCommand.AddParameter(mFields[i].ParameterName, mFields[i].Value);
                }
            }
            Where.Parse(mCommand);
            return cc.ExecuteNonQuery(mCommand);

        }

        #endregion
    }

    /// <summary>
    /// 删除数据命令
    /// </summary>
    public class Delete : ICommandExecute
    {

        public Delete(string table)
        {

            mTable = table;

        }
        private string mTable;
        private Expression mWhere = new Expression();
        public Expression Where
        {
            get
            {
                return mWhere;
            }
            set
            {
                mWhere = value;
            }
        }
        #region ICommandExecute 成员
        public int Execute()
        {
            using (IConnectinContext cc = DBContext.DB1)
            {
                return Execute(cc);
            }
        }
        public int Execute(IConnectinContext cc)
        {
            Command mCommand = Command.GetThreadCommand().AddSqlText("Delete from ").AddSqlText(mTable);
            Where.Parse(mCommand);
            return cc.ExecuteNonQuery(mCommand);

        }

        #endregion
    }

    /// <summary>
    /// 数据加载区间描述
    /// </summary>
    [Serializable]
    public class Region
    {
        public Region()
        {
        }
        public Region(int pageindex, int size)
        {
            Size = size;
            Start = pageindex * size;
        }
        public int Start
        {
            get;
            set;
        }
        public int Size
        {
            get;
            set;
        }
    }





}
