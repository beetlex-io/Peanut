using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
namespace Peanut.Validates
{
    /// <summary>
    /// 验证异常信息类
    /// </summary>
    public class ValidaterException :PeanutException
    {
        /// <summary>
        /// 构建异常
        /// </summary>
        public ValidaterException() { }
        /// <summary>
        /// 构建指定信息的异常
        /// </summary>
        /// <param name="err">错误信息</param>
        public ValidaterException(string err) : base(err) { }
        /// <summary>
        /// 构建指定信息和内部错误的异常
        /// </summary>
        /// <param name="err">错误信息</param>
        /// <param name="baseexc">内部错误类</param>
        public ValidaterException(string err, Exception baseexc) : base(err, baseexc) { }
    }
    /// <summary>
    /// 验证基础类特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidaterAttribute : Attribute
    {
        /// <summary>
        /// 验证实体属性值
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="source">属性所属对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="cc">数据库上下文</param>
        /// <returns>bool</returns>
        public bool Validating(object value, object source,Mappings.PropertyMapper pm,IConnectinContext cc )
        {
            return OnValidating(value, source, pm, cc);
               
        }
        /// <summary>
        /// 验证处理过程,派生类重写实现具体的验证规则
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="source">属性所属对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="cc">数据库上下文</param>
        /// <returns>bool</returns>
        protected abstract bool OnValidating(object value, object source, Mappings.PropertyMapper pm, IConnectinContext cc);
        /// <summary>
        /// 验证返回的信息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

    }
    /// <summary>
    /// 非空验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNull : ValidaterAttribute
    {
        /// <summary>
        /// 构建验证信息
        /// </summary>
        public NotNull()
        {

        }
        /// <summary>
        /// 构建验证信息,并指定相应的错误提示
        /// </summary>
        /// <param name="message">错误提示</param>
        public NotNull(string message)
        {
            Message = message;
        }
        /// <summary>
        /// 验证处理过程,派生类重写实现具体的验证规则
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="source">属性所属对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="cc">数据库上下文</param>
        /// <returns>bool</returns>
        protected override bool OnValidating(object value, object source, Mappings.PropertyMapper pm, IConnectinContext cc)
        {
            bool result= value != null && !string.IsNullOrEmpty(value.ToString());
            if (!result && string.IsNullOrEmpty(Message))
                Message = string.Format("{0}成员值不能为空!", pm.ColumnName);
            return result;
        }
    }
    /// <summary>
    /// 验证值是否符合相应的长度
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Length : ValidaterAttribute
    {
        /// <summary>
        /// 构建验证对象
        /// </summary>
        /// <param name="min">最小长度</param>
        /// <param name="max">最大长度</param>
        /// <param name="message">提示信息</param>
        public Length(string min, string max, string message)
        {
            if (!string.IsNullOrEmpty(min))
                MinLength = int.Parse(min);
            if (!string.IsNullOrEmpty(max))
                MaxLength = int.Parse(max);
            Message = message;
        }
        /// <summary>
        /// 最小长度
        /// </summary>
        public int? MinLength
        {
            get;
            set;
        }
        /// <summary>
        /// 最大长度
        /// </summary>
        public int? MaxLength
        {
            get;
            set;
        }
        /// <summary>
        /// 验证处理过程,派生类重写实现具体的验证规则
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="source">属性所属对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="cc">数据库上下文</param>
        /// <returns>bool</returns>
        protected override bool OnValidating(object value, object source, Mappings.PropertyMapper pm, IConnectinContext cc)
        {
           
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                string data = Convert.ToString(value);
                if (MinLength != null && MinLength > data.Length)
                {

                    return false;
                }

                if (MaxLength != null && data.Length > MaxLength)
                {

                    return false;

                }
            }
            return true;
        }
      
    }
    /// <summary>
    /// 数值区间验证类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NumberRegion : ValidaterAttribute
    {

        /// <summary>
        /// 构建数值区间验证类
        /// </summary>
        /// <param name="min">最少值</param>
        /// <param name="max">最大值</param>
        /// <param name="message">提示消息</param>
        public NumberRegion(string min, string max, string message)
        {
         
            if (!string.IsNullOrEmpty(min))
                MinValue = int.Parse(min);
            if (!string.IsNullOrEmpty(max))
                MaxValue = int.Parse(max);
            Message = message;
        }
        /// <summary>
        /// 最小值
        /// </summary>
        public int? MinValue
        {
            get;
            set;
        }
        /// <summary>
        /// 最大值
        /// </summary>
        public int? MaxValue
        {
            get;
            set;
        }
        /// <summary>
        /// 验证处理过程,派生类重写实现具体的验证规则
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="source">属性所属对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="cc">数据库上下文</param>
        /// <returns>bool</returns>
        protected override bool OnValidating(object value, object source, Mappings.PropertyMapper pm, IConnectinContext cc)
        {
            
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                int data = Convert.ToInt16(value);
                if (MinValue != null)
                {
                    if (MinValue > data)
                    {
                        return false;
                    }
                }

                if (MaxValue != null)
                {
                    if (data > MaxValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
    }
    /// <summary>
    /// 日期区间验证类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DateRegion : ValidaterAttribute
    {
        /// <summary>
        /// 构建日期验证类
        /// </summary>
        /// <param name="mindate">最小日期值</param>
        /// <param name="maxdate">最大日期值</param>
        /// <param name="message">提示消息</param>
        public DateRegion(string mindate, string maxdate, string message)
        {
            if (!string.IsNullOrEmpty(mindate))
                MinValue = DateTime.Parse(mindate);
            if (!string.IsNullOrEmpty(maxdate))
                MaxValue = DateTime.Parse(maxdate);

            Message = message;
        }
        /// <summary>
        /// 最小日期值
        /// </summary>
        public DateTime? MinValue
        {
            get;
            set;
        }
        /// <summary>
        /// 最大日期值
        /// </summary>
        public DateTime? MaxValue
        {
            get;
            set;
        }
        /// <summary>
        /// 验证处理过程,派生类重写实现具体的验证规则
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="source">属性所属对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="cc">数据库上下文</param>
        /// <returns>bool</returns>
        protected override bool OnValidating(object value, object source, Mappings.PropertyMapper pm, IConnectinContext cc)
        {
           
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                DateTime data = Convert.ToDateTime(value);
                if (MinValue != null)
                {
                    if (MinValue >data)
                    {
                        return false;
                    }
                }

                if (MaxValue != null)
                {
                    if (data > MaxValue)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
    }
    /// <summary>
    /// 正则匹配验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Match : ValidaterAttribute
    {
        /// <summary>
        /// 构建正则匹配验证
        /// </summary>
        /// <param name="regex">正则表达式</param>
        /// <param name="message">提示消息</param>
        public Match(string regex, string message)
        {
            Regex = regex;
            Message = message;
        }
        /// <summary>
        /// 获取或设置正则表达式
        /// </summary>
        public string Regex
        {
            get;
            set;
        }
        /// <summary>
        /// 验证处理过程,派生类重写实现具体的验证规则
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="source">属性所属对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="cc">数据库上下文</param>
        /// <returns>bool</returns>
        protected override bool OnValidating(object value, object source, Mappings.PropertyMapper pm, IConnectinContext cc)
        {
           
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                string data = Convert.ToString(value);
                if (System.Text.RegularExpressions.Regex.Match(
                    data, Regex, RegexOptions.IgnoreCase).Length == 0)
                {
                    return false;
                }

            }
            return true;
        }
       
    }
    /// <summary>
    /// 邮件验证类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EMail : Match
    {
        /// <summary>
        /// 构建验证类
        /// </summary>
        /// <param name="msg">提示信息</param>
        public EMail(string msg) : base(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", msg) { }
    }
    /// <summary>
    /// 身份证验证类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CardID : Match
    {
        /// <summary>
        /// 构建验证类
        /// </summary>
        /// <param name="msg">提示消息</param>
        public CardID(string msg) : base(@"^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$/", msg) { }
    }
    /// <summary>
    /// 唯一值验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class Unique : ValidaterAttribute
    {
        /// <summary>
        /// 构建验证器
        /// </summary>
        /// <param name="err">提示消息</param>
        public Unique(string err)
        {
            Message = err;
        }
        /// <summary>
        /// 验证处理过程,派生类重写实现具体的验证规则
        /// </summary>
        /// <param name="value">属性值</param>
        /// <param name="source">属性所属对象</param>
        /// <param name="pm">属性映射描述</param>
        /// <param name="cc">数据库上下文</param>
        /// <returns>bool</returns>
        protected override bool OnValidating(object value, object source, Mappings.PropertyMapper pm, IConnectinContext cc)
        {
            if (value == null)
                return true;
            if (string.IsNullOrEmpty((string)value))
                return true;
            string sql = "select {0} from {1} where {0}=@p1";
            Command cmd = new Command(string.Format(sql, pm.ColumnName, pm.OM.Table));
            cmd.AddParameter("p1", value);
            object result = cc.ExecuteScalar(cmd);
            return result == null || result == DBNull.Value;

        }
    }
    /// <summary>
    /// 验证错误
    /// </summary>
    public class ValidaterError
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get;
            set;
        }
    }
}
