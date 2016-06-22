using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace Peanut
{
    /// <summary>
    /// 数据库设备描述接口
    /// </summary>
    public interface IDriver
    {
        /// <summary>
        /// 获取数据连接类型
        /// </summary>
        IDbConnection Connection
        {
            get;
        }
        /// <summary>
        /// 获取相应命令的数据适配器
        /// </summary>
        /// <param name="cmd">数据命令</param>
        /// <returns>数据适配器</returns>
        IDbDataAdapter DataAdapter(IDbCommand cmd);
        /// <summary>
        /// 获取数据命令
        /// </summary>
        IDbCommand Command
        {
            get;
        }
        /// <summary>
        /// 替换SQL描述,主要用于处理不同数据库的一些SQL特性
        /// </summary>
        /// <param name="sql">原始SQL语句</param>
        /// <returns>处理后的SQL语句</returns>
        string ReplaceSql(string sql);
        /// <summary>
        /// 创建存储过程参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数类型</param>
        /// <returns>参数对象</returns>
        System.Data.IDataParameter CreateProcParameter(string name, object value, ParameterDirection direction);
        /// <summary>
        /// 创建命令参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数类型</param>
        /// <returns>参数对象</returns>
        System.Data.IDataParameter CreateParameter(string name, object value, ParameterDirection direction);

    }
}
