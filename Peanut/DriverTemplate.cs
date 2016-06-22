using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
namespace Peanut
{
    /// <summary>
    /// 数据库提供者模板
    /// </summary>
    /// <typeparam name="Conn">数据库连接对象</typeparam>
    /// <typeparam name="Cmd">数据库命令执行对象</typeparam>
    /// <typeparam name="Adapter">数据适配器</typeparam>
    /// <typeparam name="Parameter">命令参数对象</typeparam>
    /// <typeparam name="Sqlbuilder">Sql构建器</typeparam>
    public class DriverTemplate<Conn, Cmd, Adapter, Parameter, Sqlbuilder> : IDriver
        where Conn : IDbConnection, new()
        where Cmd : IDbCommand, new()
        where Adapter : IDbDataAdapter, new()
        where Parameter : IDataParameter, new()
        where Sqlbuilder : ISQLBuilder, new()
    {
        #region IDriver 成员
        /// <summary>
        /// SQL编译器
        /// </summary>
        protected ISQLBuilder mBuilder = new Sqlbuilder();
        /// <summary>
        /// 获取数据访问信息
        /// </summary>
        public IDbConnection Connection
        {
            get { return new Conn(); }
        }
        /// <summary>
        /// 获取相应命令的数据适配器
        /// </summary>
        /// <param name="cmd">SQL命令对象</param>
        /// <returns>数据适配器</returns>
        public IDbDataAdapter DataAdapter(IDbCommand cmd)
        {
            IDbDataAdapter da = new Adapter();
            da.SelectCommand = cmd;
            return da;
        }
        /// <summary>
        /// 获取命令对象
        /// </summary>
        public IDbCommand Command
        {
            get { return new Cmd(); }
        }
        /// <summary>
        /// 替换处理SQL语句
        /// </summary>
        /// <param name="sql">原始SQL语句</param>
        /// <returns>处理后的SQL语句</returns>
        public string ReplaceSql(string sql)
        {
            return mBuilder.ReplaceSql(sql);
        }
        /// <summary>
        /// 创建存储过程参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数类型</param>
        /// <returns>参数对象</returns>
        public virtual IDataParameter  CreateProcParameter(string name, object value, ParameterDirection direction)
        {
            IDataParameter dp = new Parameter();
            mBuilder.SetProcParameter(dp, name, value, direction);
            return dp;
        }
        /// <summary>
        /// 创建命令参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数类型</param>
        /// <returns>参数对象</returns>
        public virtual IDataParameter CreateParameter(string name, object value, ParameterDirection direction)
        {
            IDataParameter dp = new Parameter();
            mBuilder.SetParameter(dp, name, value, direction);
            return dp;
        }

        #endregion
    }
}
