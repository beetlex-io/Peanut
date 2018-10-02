using System;
using System.Collections.Generic;
using System.Text;


namespace Peanut.Mappings
{
    /// <summary>
    /// 实体属性映射描述
    /// </summary>
    public class PropertyMapper
    {
        /// <summary>
        /// 构建映射类
        /// </summary>
        public PropertyMapper()
        {
           
        }
        //public int ColumnIndex
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }
        /// <summary>
        /// 属性访问的句柄
        /// </summary>
        public PropertyHandler Handler
        {
            get;
            set;
        }
        /// <summary>
        /// 值转换器
        /// </summary>
        public PropertyCastAttribute Cast
        {
            get;
            set;
        }
        /// <summary>
        /// 属性汇总描述
        /// </summary>
        public AggregationAttribute Aggregation
        {
            get;
            set;
        }
        /// <summary>
        /// 默认值描述
        /// </summary>
        public ValueAttribute Value
        {
            get;
            set;
        }
        /// <summary>
        /// 匹配对象是否相等
        /// </summary>
        /// <param name="obj">匹配对象</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            PropertyMapper pm = (PropertyMapper)obj;
            return pm.ColumnName == this.ColumnName;
            
        }
        internal ObjectMapper OM
        {
            get;
            set;
        }
        /// <summary>
        /// 属性相应的验证描述
        /// </summary>
        public Validates.ValidaterAttribute[] Validaters
        {
            get;
            set;
        }
    }
}
