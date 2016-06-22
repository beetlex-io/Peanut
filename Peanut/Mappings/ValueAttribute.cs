using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut.Mappings
{
    /// <summary>
    /// 默认值描述抽象类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValueAttribute:Attribute
    {
        /// <summary>
        /// 构建默认值描述
        /// </summary>
        /// <param name="afterupdate">是否添加数据到数据后才处理</param>
        public ValueAttribute(bool afterupdate)
        {
            AfterByUpdate = afterupdate;   
        }
        /// <summary>
        /// 是否添加数据到数据后才处理
        /// </summary>
        public bool AfterByUpdate
        {
            get;
            set;
        }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public virtual void Executing(IConnectinContext cc,object data,PropertyMapper pm,string table)
        {
        }
        /// <summary>
        /// 数据保存后处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public virtual void Executed(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
        }
    }
    /// <summary>
    /// MSSQL自增长值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IDENTITY : ValueAttribute
    {
        /// <summary>
        /// 构建自增长值
        /// </summary>
        public IDENTITY()
            : base(true)
        {
        }
        /// <summary>
        /// 数据保存后处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executed(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            Command cmd = new Command("select @@IDENTITY ");
            object value = cc.ExecuteScalar(cmd);
            pm.Handler.Set(data,Convert.ChangeType( value,pm.Handler.Property.PropertyType));
        }
    }
    /// <summary>
    /// GUID默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UID:ValueAttribute
    {
        /// <summary>
        /// 构建GUID默认值
        /// </summary>
        public UID() : base(false) { }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            string uid = Guid.NewGuid().ToString("N");
            pm.Handler.Set(data, uid);
        }
    }
    /// <summary>
    /// 当前年月默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class YearMonth : ValueAttribute
    {
        /// <summary>
        /// 构建当前年月默认值
        /// </summary>
        public YearMonth() : base(false) { }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00"));
        }
    }
    /// <summary>
    /// 获取当前年默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Year : ValueAttribute
    {
        /// <summary>
        /// 当前年默认值
        /// </summary>
        public Year() : base(false) { }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, DateTime.Now.Year.ToString());
        }
    }
    /// <summary>
    /// 当前月默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Month : ValueAttribute
    {
        /// <summary>
        /// 构建当前月默认值
        /// </summary>
        public Month() : base(false) { }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, DateTime.Now.Month.ToString());
        }
    }
    /// <summary>
    /// 当前日默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Day : ValueAttribute
    {
        /// <summary>
        /// 构建当前月默认值
        /// </summary>
        public Day() : base(false) { }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, DateTime.Now.Day.ToString());
        }
    }
    /// <summary>
    /// 当前日期默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NowDate:ValueAttribute
    {
        /// <summary>
        /// 构建当前日期默认值
        /// </summary>
        public NowDate() : base(false) { }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, DateTime.Now);
        }
    }
    /// <summary>
    /// 指定默认值int
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultInt : ValueAttribute
    {
        private int mValue = 0;
        /// <summary>
        /// 构建默认值int
        /// </summary>
        /// <param name="value">具体数值</param>
        public DefaultInt(int value) : base(false) {
            mValue = value;
        }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, mValue);
        }
    }
    /// <summary>
    /// 默认值数值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultDecimal : ValueAttribute
    {
        private decimal mValue = 0;
        /// <summary>
        /// 构建默认值数值
        /// </summary>
        /// <param name="value">具体数的字符描述</param>
        public DefaultDecimal(string value)
            : base(false)
        {
            mValue = Convert.ToDecimal(value);
        }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data,mValue);
        }
    }
    /// <summary>
    /// 默认字符值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultString : ValueAttribute
    { 
        private string mValue = "";
        /// <summary>
        /// 构建默认字符值
        /// </summary>
        /// <param name="value">相应的字符值</param>
        public DefaultString(string value)
            : base(false)
        {
            mValue = value;
        }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, mValue);
        }
    }
    /// <summary>
    /// 默认日期
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultDate:ValueAttribute
    {
        private DateTime mValue = DateTime.MinValue;
        /// <summary>
        /// 构建默认日期
        /// </summary>
        /// <param name="value">相应日期的字符表达值</param>
        public DefaultDate(string value)
            : base(false)
        {
            mValue =DateTime.Parse(value);
        }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, mValue);
        }
    }
    /// <summary>
    /// 布尔值默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Enabled : ValueAttribute
    {
        /// <summary>
        /// 构建布尔值默认值
        /// </summary>
        public Enabled() : base(false) { }
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            pm.Handler.Set(data, true);
        }
    }
    /// <summary>
    /// 枚举默认值
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultEnum : ValueAttribute
    {
        /// <summary>
        /// 构建默认枚举值
        /// </summary>
        public DefaultEnum() : base(false) { }
        /// <summary>
        /// 构建默认枚举值
        /// </summary>
        /// <param name="value">相应Int的枚举值</param>
        public DefaultEnum(int value): base(false) 
        {
            mValue = value;
            mInputType = InputType.Int;
        }
        /// <summary>
        /// 构建默认枚举值
        /// </summary>
        /// <param name="value">相应String的枚举值</param>
        public DefaultEnum(string value)
            : base(false)
        {
            mValue = value;
            mInputType = InputType.String;
            
        }
        private InputType mInputType = InputType.None;
        private object mValue;
        /// <summary>
        /// 数据保存前处理过程
        /// </summary>
        /// <param name="cc">数据库上下文</param>
        /// <param name="data">实体对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="table">相关表信息</param>
        public override void Executing(IConnectinContext cc, object data, PropertyMapper pm, string table)
        {
            if (mInputType == InputType.Int)
            {
                pm.Handler.Set(data, Enum.GetValues(pm.Handler.Property.PropertyType).GetValue((int)mValue));
            }
            else if (mInputType == InputType.String)
            {
                Enum.Parse(pm.Handler.Property.PropertyType, mValue.ToString());
            }
            else
            {
                pm.Handler.Set(data, Enum.GetValues(pm.Handler.Property.PropertyType).GetValue(0));
            }
        }
        enum InputType
        { 
            None,
            Int,
            String
        }
    }


}
