using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
namespace Peanut
{
    public class SqlitBuilder : ISQLBuilder
    {
        public string ReplaceSql(string sql)
        {
            //return Regex.Replace(sql, "@[a-zA-z_0-9]+", "?", RegexOptions.IgnoreCase);
            return sql;
        }

        public void SetParameter(IDataParameter dp, string name, object value, ParameterDirection direction)
        {
            dp.ParameterName = "@"+name;
            if (value == null)
                dp.Value = DBNull.Value;
            else
                dp.Value = value;
            dp.Direction = direction;

        }


        public void SetProcParameter(IDataParameter dp, string name, object value, ParameterDirection direction)
        {
            dp.ParameterName = "@" + name;
            if (value == null)
                dp.Value = DBNull.Value;
            else
                dp.Value = value;
            dp.Direction = direction;
        }
    }
}
