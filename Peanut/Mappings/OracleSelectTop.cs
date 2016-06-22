using System;
using System.Collections.Generic;

using System.Text;

namespace Peanut.Mappings
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class OracleSelectTop : Peanut.Mappings.SelectChangeAttribute
    {
        public OracleSelectTop(int max)
        {
            Max = max;
        }
        public int Max
        {
            get;
            set;
        }
        public override void Change(Peanut.Command cmd)
        {
            string sql =string.Format("select * from ({0}) where rownum <{1}",cmd.Text.ToString(),Max);
#if NET_4
            cmd.Text.Clear();
#else
            cmd.Text.Remove(0, cmd.Text.Length);
#endif
           
            cmd.Text.Append(sql);
           
        }
    }
}
