using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut
{
    /// <summary>
    /// 条件表达式
    /// </summary>
    [Serializable]
    public partial class Expression
    {
        public Expression() 
        {
            
        }
        private List<Command.Parameter> mParameters = new List<Command.Parameter>();
        public List<Command.Parameter> Parameters
        {
            get
            {
                return mParameters;
            }
        }
        private StringBuilder mSqlText = new StringBuilder();
        public StringBuilder SqlText
        {
            get
            {
                return mSqlText;
            }
             
        }
        internal void Parse(Command cmd)
        {
            if (SqlText.Length > 0)
            {
                cmd.Text.Append(" where ").Append(SqlText.ToString());
                for(int i =0;i<Parameters.Count;i++)
                {
                    Command.Parameter p= Parameters[i];
               
                    cmd.AddParameter(p);
                }
            }
        }
        public static Expression operator &(Expression exp1, Expression exp2)
        {
            if (exp1 == null || exp1.SqlText.Length == 0)
                return exp2;
            if (exp2 == null || exp2.SqlText.Length == 0)
                return exp1;

            Expression exp = new Expression();
           // exp.ParameterNameIndex = exp1.ParameterNameIndex>exp2.ParameterNameIndex?exp1.ParameterNameIndex:exp2.ParameterNameIndex;
            exp.SqlText.Append("(");
            exp.SqlText.Append(exp1.ToString());

            exp.SqlText.Append(")");
            exp.Parameters.AddRange(exp1.Parameters);
            exp.SqlText.Append(" and (");
            exp.SqlText.Append(exp2.SqlText.ToString());
            exp.SqlText.Append(")");
            exp.Parameters.AddRange(exp2.Parameters);
            return exp;
        }
        public static Expression operator |(Expression exp1, Expression exp2)
        {
            if (exp1 == null || exp1.SqlText.Length == 0)
                return exp2;
            if (exp2 == null || exp2.SqlText.Length == 0)
                return exp1;
            Expression exp = new Expression();
           // exp.ParameterNameIndex = exp1.ParameterNameIndex > exp2.ParameterNameIndex ? exp1.ParameterNameIndex : exp2.ParameterNameIndex;
            exp.SqlText.Append("(");
            exp.SqlText.Append(exp1.ToString());
           
            exp.SqlText.Append(")");
            exp.Parameters.AddRange(exp1.Parameters);
            exp.SqlText.Append(" or (");
            exp.SqlText.Append(exp2.SqlText.ToString());
            exp.SqlText.Append(")");
            exp.Parameters.AddRange(exp2.Parameters);
            return exp;

        }

        internal static string GetParamName()
        {


            ParamNameSeed pns = NameSeed;
            //if (pns.Value > 200)
            //    pns.Value = 0;
            //else
             pns.Value++;
            return "tmp_p" + pns.Value;


        }
        [ThreadStatic]
        static ParamNameSeed mNameSeed = new ParamNameSeed();
        internal static ParamNameSeed NameSeed
        {
            get
            {
                if (mNameSeed == null)
                    mNameSeed = new ParamNameSeed();
                return mNameSeed;
            }
        }
        internal class ParamNameSeed
        {
            public int Value
            {
                get;
                set;
            }
        }
        public override string ToString()
        {

            return SqlText.ToString();
        }

        public Expression AddSql(string sql)
        {
            mSqlText.Append(sql);
            return this;
        }

        public Expression Add(string name, object value)
        {
            mParameters.Add(new Command.Parameter { Name= name, Value = value });
            return this;
        }
        
    }
   
}
