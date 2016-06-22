using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    /// <summary>
    /// ID映射描述,主要对应表的唯一索引字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IDAttribute:Attribute
    {
        /// <summary>
        /// 构建ID映射
        /// </summary>
        public IDAttribute()
        {
           
        }
        /// <summary>
        /// 构建ID映射,并指定对应的字段名
        /// </summary>
        /// <param name="name">字段名</param>
        public IDAttribute(string name)
        {
            Name = name;
        }
        /// <summary>
        /// 获取或设置字段名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
