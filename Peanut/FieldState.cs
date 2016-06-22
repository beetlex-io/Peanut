using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut
{
    /// <summary>
    /// 字段状态描述
    /// </summary>
    [Serializable]
    public class FieldState
    {
        /// <summary>
        /// 获取或设置修改的时间
        /// </summary>
        public DateTime ModifyTime
        {
            get;
            set;
        }
    }
}
