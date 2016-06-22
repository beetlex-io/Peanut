using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace Peanut
{

    public class OracleDriver : DriverTemplate<
      OracleConnection, OracleCommand, OracleDataAdapter, OracleParameter, OracleBuilder>
    {
        public enum OracleProcCursor
        {
            None
        }
      
        public override System.Data.IDataParameter CreateProcParameter(string name, object value, System.Data.ParameterDirection direction)
        {
            System.Data.IDataParameter dp = base.CreateProcParameter(name, value, direction);
            if (direction == System.Data.ParameterDirection.Output)
            {
                if (value !=null && value.GetType()== typeof(OracleProcCursor))
                {
                    OracleParameter op = (OracleParameter)dp;
                    op.OracleType = OracleType.Cursor;
                    op.Value = DBNull.Value;
                }
            }
            return dp;
        }
    }
}
