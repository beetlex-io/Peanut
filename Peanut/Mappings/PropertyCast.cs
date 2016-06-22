using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    /// <summary>
    /// 属性值转换描述抽象类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Class|AttributeTargets.Enum|  AttributeTargets.Struct)]
    public abstract class PropertyCastAttribute:Attribute
    {
        /// <summary>
        /// 数据库到对象属性转换,派生类重写实现自定义转换规则
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="ptype">对象类型</param>
        /// <param name="source">所属对象</param>
        /// <returns>转换后的值</returns>
        public abstract object ToProperty(object value,Type ptype,object source);
        /// <summary>
        /// 对象属性到数据库,派生类重写实现自定义转换规则
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="ptype">对象类型</param>
        /// <param name="source">所属对象</param>
        /// <returns>转换后的值</returns>
        public abstract object ToColumn(object value, Type ptype, object source);
        internal static object CastValue(string table, string column, object value)
        {
            ObjectMapper om = ObjectMapper.GetOM(table);
            return CastValue(om, column, value);
        }
        internal static object CastValue(ObjectMapper om, string column, object value)
        {

            PropertyMapper pm;
            if (om == null)
                return value;
            pm = om[column];
            if (pm != null && pm.Cast != null)
                return pm.Cast.ToColumn(value, pm.Handler.Property.PropertyType, null);


            return value;
        }
    }
    /// <summary>
    /// 枚举和字符的转换器
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    public class EnumToString : PropertyCastAttribute
    {
        /// <summary>
        /// 数据库到对象属性转换,派生类重写实现自定义转换规则
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="ptype">对象类型</param>
        /// <param name="source">所属对象</param>
        /// <returns>转换后的值</returns>
        public override object ToProperty(object value, Type ptype,object source)
        {
            return Enum.Parse(ptype, value.ToString());
        }
        /// <summary>
        /// 对象属性到数据库,派生类重写实现自定义转换规则
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="ptype">对象类型</param>
        /// <param name="source">所属对象</param>
        /// <returns>转换后的值</returns>
        public override object ToColumn(object value, Type ptype, object source)
        {
            return value.ToString();
        }
    }
    /// <summary>
    /// 枚举和整型的转换器
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    public class EnumToInt : PropertyCastAttribute
    {
        /// <summary>
        /// 对象属性到数据库,派生类重写实现自定义转换规则
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="ptype">对象类型</param>
        /// <param name="source">所属对象</param>
        /// <returns>转换后的值</returns>
        public override object ToColumn(object value, Type ptype, object source)
        {
            return value.GetHashCode();
        }
        /// <summary>
        /// 数据库到对象属性转换,派生类重写实现自定义转换规则
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="ptype">对象类型</param>
        /// <param name="source">所属对象</param>
        /// <returns>转换后的值</returns>
        public override object ToProperty(object value, Type ptype, object source)
        {
            int result = Convert.ToInt32(value);
            Array values = Enum.GetValues(ptype);
            for (int i = 0; i < values.Length; i++)
            {
                if (values.GetValue(i).GetHashCode() == result)
                {
                    return values.GetValue(i);
                }
            }
            return values.GetValue(0);
            
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class RelationToOne : PropertyCastAttribute
    {
        public RelationToOne(string column)
        {
            Column = column;
        }
        public string Column
        {
            get;
            set;
        }
        public override object ToColumn(object value, Type ptype,object source)
        {
            ObjectMapper om = ObjectMapper.GetOM(ptype);
            PropertyMapper pm = om[Column];
            return pm.Handler.Get(value);
        }
        public override object ToProperty(object value, Type ptype,object source)
        {
            ObjectMapper om = ObjectMapper.GetOM(ptype);
            ObjectMapper otherom = ObjectMapper.GetOM(source.GetType());
            PropertyMapper otherpm = otherom[Column];

            return null;
        }

    }
    /// <summary>
    /// 基于BlowFish加密的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct)]
    public class StringCrypto : PropertyCastAttribute
    {
        /// <summary>
        /// 默认的加密KEY
        /// </summary>
        public static string BlowfishKey="sdfsdfsd1213lkjl";
   
        private BlowFishCS.BlowFish mBlowfish;
        /// <summary>
        /// 构建加密属性
        /// </summary>
        public StringCrypto()
        {
            mBlowfish = new BlowFishCS.BlowFish(BlowfishKey);
        }
        /// <summary>
        /// 构建加密属性,并指定相应的加密KEY
        /// </summary>
        /// <param name="key">加密KEY</param>
        public StringCrypto(string key)
        {
            mBlowfish = new BlowFishCS.BlowFish(key);
        }
        /// <summary>
        /// 数据库到对象属性转换,派生类重写实现自定义转换规则
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="ptype">对象类型</param>
        /// <param name="source">所属对象</param>
        /// <returns>转换后的值</returns>
        public override object ToProperty(object value, Type ptype, object source)
        {

            return DecryptString((string)value);
        }
        /// <summary>
        /// 对象属性到数据库,派生类重写实现自定义转换规则
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="ptype">对象类型</param>
        /// <param name="source">所属对象</param>
        /// <returns>转换后的值</returns>
        public override object ToColumn(object value, Type ptype, object source)
        {

            return EncryptString((string)value);
        }
        /// <summary>
        /// 加密字符
        /// </summary>
        /// <param name="value">需要加密的信息</param>
        /// <returns>string</returns>
        public string EncryptString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return mBlowfish.Encrypt_CTR(value);
        }
        /// <summary>
        /// 解密字符
        /// </summary>
        /// <param name="value">需要解密的信息</param>
        /// <returns>string</returns>
        public string DecryptString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return mBlowfish.Decrypt_CTR(value);
        }
    }
    
}
