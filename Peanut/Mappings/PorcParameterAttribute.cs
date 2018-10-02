using System;
using System.Collections.Generic;
using System.Text;


namespace Peanut.Mappings
{
    /// <summary>
    /// 存储过程参数描述
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ProcParameterAttribute:Attribute
    {
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 参数类型
        /// </summary>
        public System.Data.ParameterDirection Direction
        {
            get;
            set;
        }
        /// <summary>
        /// 是否输出参数
        /// </summary>
        public bool Output
        {
            get;
            set;
        }
        /// <summary>
        /// 属性操作句柄
        /// </summary>
        public PropertyHandler Handler
        {
            get;
            set;
        }
    }
}
