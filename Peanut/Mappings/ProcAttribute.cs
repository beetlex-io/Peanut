using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    /// <summary>
    /// 存储过程描述对象
    /// </summary>
    public class ProcAttribute:Attribute
    {
        /// <summary>
        /// 相应的存储过程名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
