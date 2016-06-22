using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace Peanut
{
    /// <summary>
    /// mysql编译器
    /// </summary>
    public class MysqlBuilder : ISQLBuilder
    {
        #region ISQLBuilder 成员

        /// <summary>
        /// 替换SQL描述,主要用于处理不同数据库的一些SQL特性
        /// </summary>
        /// <param name="sql">原始SQL语句</param>
        /// <returns>处理后的SQL语句</returns>
        public string ReplaceSql(string sql)
        {
            return sql;
        }
        /// <summary>
        /// 创建命令参数
        /// </summary>
        /// <param name="dp">参数对象</param>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数类型</param>
        /// <returns>参数对象</returns>
        public void SetProcParameter(IDataParameter dp, string name, object value, ParameterDirection direction)
        {
            dp.ParameterName = "@" + name;
            if (value == null)
                dp.Value = DBNull.Value;
            else
                dp.Value = value;
            dp.Direction = direction;
        }
        /// <summary>
        /// 创建存储过程参数
        /// </summary>
        /// <param name="dp">参数对象</param>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="direction">参数类型</param>
        /// <returns>参数对象</returns>
        public void SetParameter(IDataParameter dp, string name, object value, ParameterDirection direction)
        {
            dp.ParameterName = "@" + name;
            if (value == null)
                dp.Value = DBNull.Value;
            else
                dp.Value = value;
            dp.Direction = direction;

        }

        #endregion
    }
}
