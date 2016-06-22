using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut
{
    /// <summary>
    /// 基于内嵌表的表达式,用于代替id in(select id in table where expression)
    /// </summary>
    public class InTableExpression
    {
        /// <summary>
        /// in的字段信息
        /// </summary>
        public IFieldInfo Field
        {
            get;
            set;
        }
        /// <summary>
        /// 相应字段的表达式
        /// </summary>
        public Expression Expression
        {
            get;
            set;
        }
    }
}
