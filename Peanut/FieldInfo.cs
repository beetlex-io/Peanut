using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut
{
    /// <summary>
    /// 字段信息描述类
    /// </summary>
    public interface IFieldInfo
    {
        /// <summary>
        /// 获取相关表信息
        /// </summary>
        string Table
        {
            get;
        }
        /// <summary>
        /// 获取相关字段名称
        /// </summary>
        string Name
        {
            get;
        }
        /// <summary>
        /// 获取相应字段的DESC排序规则
        /// </summary>
        string Desc
        {
            get;
        }
        /// <summary>
        /// 获取相应字段的ASC排序规则
        /// </summary>
        string Asc
        {
            get;
        }
    }
    /// <summary>
    /// 字段描述对象
    /// </summary>
    /// <typeparam name="T">相应字段的数据类型</typeparam>
    public class FieldInfo<T>:IFieldInfo
    {
        /// <summary>
        /// 构建字段信息
        /// </summary>
        /// <param name="table">相应的表名称</param>
        /// <param name="name">字段名称</param>
        public FieldInfo(string table, string name)
        {
            DBContext.Init();
            mTable = table;
            mName = name;
        }
        private string mTable;
        /// <summary>
        /// 获取字段所在的表名称
        /// </summary>
        public string Table
        {
            get
            {
                return mTable;
            }
        }
        private string mName;
        /// <summary>
        /// 获取字段名称
        /// </summary>
        public string Name
        {
            get
            {
                return mName;
            }
        }
        /// <summary>
        /// 获取字段等于一个值的表达式
        /// </summary>
        /// <param name="value">对象值</param>
        /// <returns>条件表达式</returns>
        public Expression Eq(T value)
        {
           
            Expression exp = new Expression();
            string p = Expression.GetParamName();
            exp.SqlText.Append(string.Format(" {0}=@{1} ", Name, p));
            exp.Parameters.Add(new Command.Parameter
            {
                Name = p,
                Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value)
            });
            return exp;
        }
        /// <summary>
        /// 获取字段小于等于一个值的表达式
        /// </summary>
        /// <param name="value">对象值</param>
        /// <returns>条件表达式</returns>
        public Expression LtEq(T value)
        {
            //string p = Expression.GetParamName();
            Expression exp = new Expression();
            string p = Expression.GetParamName();
            exp.SqlText.Append(string.Format(" {0}<=@{1} ", Name, p));
            exp.Parameters.Add(new Command.Parameter
            {
                Name = p,
                Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value)
            });
            return exp;
        }
        /// <summary>
        /// 获取字段小于一个值的表达式
        /// </summary>
        /// <param name="value">对象值</param>
        /// <returns>条件表达式</returns>
        public Expression Lt(T value)
        {
           // string p = Expression.GetParamName();
            Expression exp = new Expression();
            string p = Expression.GetParamName();
            exp.SqlText.Append(string.Format(" {0}<@{1} ", Name, p));
            exp.Parameters.Add(new Command.Parameter
            {
                Name = p,
                Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value)
            });
            return exp;
        }

        /// <summary>
        /// 获取一个关联表
        /// </summary>
        /// <param name="field">关联字段</param>
        /// <returns>关联表</returns>
        public JoinTable LeftJoin(FieldInfo<T> field)
        {
            return new JoinTable(this, field, JoinType.Left);
        }
        /// <summary>
        /// 获取一个关联表
        /// </summary>
        /// <param name="field">关联字段</param>
        /// <returns>关联表</returns>
        public JoinTable InnerJoin(FieldInfo<T> field)
        {
            return new JoinTable(this, field, JoinType.Inner);
        }
        /// <summary>
        /// 获取一个关联表
        /// </summary>
        /// <param name="field">关联字段</param>
        /// <returns>关联表</returns>
        public JoinTable RightJoin(FieldInfo<T> field)
        {
            return new JoinTable(this, field, JoinType.Right);
        }
        /// <summary>
        /// 获取一个关联表
        /// </summary>
        /// <param name="field">关联字段</param>
        /// <returns>关联表</returns>
        public JoinTable OuterJoin(FieldInfo<T> field)
        {
            return new JoinTable(this, field, JoinType.Outer);
        }
        /// <summary>
        /// 获取字段大于一个值的表达式
        /// </summary>
        /// <param name="value">对象值</param>
        /// <returns>条件表达式</returns>
        public Expression Gt(T value)
        {
            //string p = Expression.GetParamName();
            Expression exp = new Expression();
            string p = Expression.GetParamName();
            exp.SqlText.Append(string.Format(" {0}>@{1} ", Name, p));
            exp.Parameters.Add(new Command.Parameter
            {
                Name = p,
                Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value)
            });
            return exp;
        }
        /// <summary>
        /// 获取字段大于等于一个值的表达式
        /// </summary>
        /// <param name="value">对象值</param>
        /// <returns>条件表达式</returns>
        public Expression GtEq(T value)
        {
          
            Expression exp = new Expression();
            string p = Expression.GetParamName();
            exp.SqlText.Append(string.Format(" {0}>=@{1} ", Name, p));
            exp.Parameters.Add(new Command.Parameter { Name = p, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value) });
            return exp;
        }
        /// <summary>
        /// 获取字段不等于一个值的表达式
        /// </summary>
        /// <param name="value">对象值</param>
        /// <returns>条件表达式</returns>
        public Expression NotEq(T value)
        {
         
            Expression exp = new Expression();
            string p = Expression.GetParamName();
            exp.SqlText.Append(string.Format(" {0}<>@{1} ", Name, p));
            exp.Parameters.Add(new Command.Parameter { Name = p, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value) });
            return exp;
        }
        /// <summary>
        /// 获取字段存在一列表中的表达式
        /// </summary>
        /// <param name="value">对象值列表</param>
        /// <returns>条件表达式</returns>
        public Expression In(IEnumerable<T> values)
        {
            string p;
            int i = 0;
            Expression exp = new Expression();
            exp.SqlText.Append(" ").Append( Name).Append(" in (");
            
            foreach (object value in values)
            {
                p = Expression.GetParamName();
                if (i > 0)
                    exp.SqlText.Append(",");
                exp.SqlText.Append("@").Append(p);
                exp.Parameters.Add(new Command.Parameter { Name = p, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value) });

                i++;
            }
            exp.SqlText.Append(" )");
            return exp;
        }
        /// <summary>
        /// 获取字段和另一个表的字段并集条件表达式
        /// </summary>
        /// <param name="field">并集字段</param>
        /// <param name="expression">相应表的条件查询表达式</param>
        /// <returns>条件表达式</returns>
        public Expression In(FieldInfo<T> field, Expression expression)
        {
            Expression exp = new Expression();
            string astable = "T" + Expression.GetParamName();
            exp.SqlText.Append(" ").Append(Name).Append(" in (select ").Append(astable).Append(".").Append(field.Name).Append(" from ").Append(field.Table).Append(" ").Append( astable);
            if (expression != null && expression.SqlText.Length > 0)
            {
                exp.SqlText.Append(" where ").Append( expression.SqlText.ToString());
                exp.Parameters.AddRange(expression.Parameters);
            }
            exp.SqlText.Append(")");
            return exp;
        }
        /// <summary>
        /// 获取字段不存在列表中的表达式
        /// </summary>
        /// <param name="value">对象值列表</param>
        /// <returns>条件表达式</returns>
        public Expression NotIn(IEnumerable<T> values)
        {
            string p;
            int i = 0;
            Expression exp = new Expression();
            exp.SqlText.Append(" ").Append(Name).Append(" not in (");
            foreach (object value in values)
            {
                p = Expression.GetParamName();
                if (i > 0)
                    exp.SqlText.Append(",");
                exp.SqlText.Append("@").Append( p);
                exp.Parameters.Add(new Command.Parameter { Name = p, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value) });

                i++;
            }
            exp.SqlText.Append(")");
            return exp;
        }
        /// <summary>
        /// 获取字段和另一个表的字段非集条件表达式
        /// </summary>
        /// <param name="field">并集字段</param>
        /// <param name="expression">相应表的条件查询表达式</param>
        /// <returns>条件表达式</returns>
        public Expression NotIn(FieldInfo<T> field, Expression expression)
        {
            Expression exp = new Expression();
            string astable = "T" + Expression.GetParamName();
            exp.SqlText.Append(" ").Append(Name).Append(" not in (select ").Append(astable).Append(".").Append(field.Name).Append(" from ").Append( field.Table).Append( " ").Append(astable);
            if (expression != null && expression.SqlText.Length > 0)
            {
                exp.SqlText.Append(" where ").Append(expression.SqlText.ToString());
                exp.Parameters.AddRange(expression.Parameters);
            }
            exp.SqlText.Append(")");
            return exp;
        }
        /// <summary>
        /// 获取这段在指定区间的表达式
        /// </summary>
        /// <param name="fromvalue">开始值</param>
        /// <param name="tovalue">结束值</param>
        /// <returns>条件表达式</returns>
        public Expression Between(T fromvalue, T tovalue)
        {
            string p, p1;
           
            Expression exp = new Expression();
            p = Expression.GetParamName();
            p1 = Expression.GetParamName();
            exp.SqlText.Append(string.Format(" {0} Between @{1} and @{2}", Name, p, p1));
            exp.Parameters.Add(new Command.Parameter { Name = p, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, fromvalue) });
            exp.Parameters.Add(new Command.Parameter { Name = p1, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, tovalue) });
            return exp;
        }
        /// <summary>
        /// 获取这段不存在指定区间的表达式
        /// </summary>
        /// <param name="fromvalue">开始值</param>
        /// <param name="tovalue">结束值</param>
        /// <returns>条件表达式</returns>
        public Expression NotBetween(T fromvalue, T tovalue)
        {
            string p, p1;
           
            Expression exp = new Expression();
            p = Expression.GetParamName();
            p1 = Expression.GetParamName();
            exp.SqlText.Append(string.Format(" {0} not Between @{1} and @{2}", Name, p, p1));
            exp.Parameters.Add(new Command.Parameter { Name = p, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, fromvalue) });
            exp.Parameters.Add(new Command.Parameter { Name = p1, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, tovalue) });
            return exp;
        }
        /// <summary>
        /// 返回字段匹配某一个值的表达式
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>条件表达式</returns>
        public Expression Like(object value)
        {
            if (value != null && value is System.Collections.IEnumerable && value.GetType() != typeof(string))
                return LikeMany((System.Collections.IEnumerable)value);
           
            Expression exp = new Expression();
            string pn = Expression.GetParamName();
            exp.SqlText.Append(
                string.Format("{0} like @{1}", Name, pn)
                );
            exp.Parameters.Add(new Command.Parameter { Name = pn, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value) });
            return exp;
        }
        /// <summary>
        /// 返回字段匹配相应列表值的表达式
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>条件表达式</returns>
        private Expression LikeMany(System.Collections.IEnumerable value)
        {

            Expression exp = new Expression();
            int i = 0;
            string pn;
            exp.SqlText.Append("(");
            foreach (object item in value)
            {
                pn = Expression.GetParamName();
                if (i > 0)
                    exp.SqlText.Append(" or ");
                exp.SqlText.Append(Name).Append(" like @").Append(pn);
                exp.Parameters.Add(new Command.Parameter { Name = pn, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, item) });
                i++;
            }
            exp.SqlText.Append(")");
            return exp;
        }
        /// <summary>
        /// 返回字段匹配某一个值的表达式,此方法会在值前后添加通配符%
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>条件表达式</returns>
        public Expression Match(object value)
        {
            if (value != null && value is System.Collections.IEnumerable && value.GetType() != typeof(string))
                return MatchMany((System.Collections.IEnumerable)value);
            return Like("%" + value + "%");
        }
        /// <summary>
        /// 返回字段匹配相应列表值的表达式,此方法会在值前后添加通配符%
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>条件表达式</returns>
        private Expression MatchMany(System.Collections.IEnumerable value)
        {
            Expression exp = new Expression();
            int i = 0;
            string pn;
            exp.SqlText.Append("(");
            foreach (object item in value)
            {
                pn = Expression.GetParamName();
                if (i > 0)
                    exp.SqlText.Append(" or ");
                exp.SqlText.Append(Name).Append(" like @").Append(pn);
                exp.Parameters.Add(new Command.Parameter { Name = pn, Value = "%" + item + "%" });
                i++;
            }
            exp.SqlText.Append(")");
            return exp;
        }
        /// <summary>
        /// 返回字段为空的表达式
        /// </summary>
        /// <returns>条件表达式</returns>
        public Expression IsNull()
        {
            Expression exp = new Expression();
            exp.SqlText.Append(" ").Append(Name).Append(" is null");
            return exp;
        }
        /// <summary>
        /// 返回字段非空的表达式
        /// </summary>
        /// <returns>条件表达式</returns>
        public Expression IsNotNull()
        {
            Expression exp = new Expression();
            exp.SqlText.Append(" ").Append(Name).Append(" is not null");
            return exp;
        }
        /// <summary>
        /// 返回字段名称
        /// </summary>
        /// <returns>字符值</returns>
        public override string ToString()
        {
            return Name;
        }
        public static Expression operator ==(FieldInfo<T> field, IEnumerable<T> value)
        {
            if (value == null)
                return field.IsNull();
            return field.In(value);
        }
        public static Expression operator ==(FieldInfo<T> field, InTableExpression value)
        {
            InTableExpression ite = (InTableExpression)value;
            return field.In((FieldInfo<T>)ite.Field, ite.Expression);
        }
        public static Expression operator ==(FieldInfo<T> field, T value)
        {
            if (value == null)
                return field.IsNull();
            return field.Eq(value);
        }
        public static Expression operator !=(FieldInfo<T> field, IEnumerable<T> value)
        {
            if (value == null)
                return field.IsNotNull();
            return field.NotIn(value);
        }

        public static Expression operator !=(FieldInfo<T> field, InTableExpression value)
        {
            InTableExpression ite = (InTableExpression)value;
            return field.NotIn((FieldInfo<T>)ite.Field, ite.Expression);
        }
        public static Expression operator !=(FieldInfo<T> field, T value)
        {
            if (value == null)
                return field.IsNotNull();
          
            return field.NotEq(value);
        }
        public static Expression operator >(FieldInfo<T> field, T value)
        {
            return field.Gt(value);
        }
        public static Expression operator >=(FieldInfo<T> field, T value)
        {
            return field.GtEq(value);
        }
        public static Expression operator <(FieldInfo<T> field, T value)
        {
            return field.Lt(value);
        }
        public static Expression operator <=(FieldInfo<T> field, T value)
        {
            return field.LtEq(value);
        }
        /// <summary>
        /// 返回带表前缀的字段信息
        /// </summary>
        /// <returns>字段描述对象</returns>
        public FieldInfo<T> At()
        {
            return new FieldInfo<T>(Table, Table + "." + Name);
        }
        /// <summary>
        /// 返回指定表前缀的字段信息
        /// </summary>
        /// <param name="table">表名称</param>
        /// <returns>字段描述对象</returns>
        public FieldInfo<T> At(string table)
        {
            return new FieldInfo<T>(table, table + "." + Name);
        }
        /// <summary>
        /// 获取字段的DESC排序规则
        /// </summary>
        public string Desc
        {
            get
            {
                return Name + " desc ";
            }
        }
        /// <summary>
        /// 获取字段ASC的排序规则
        /// </summary>
        public string Asc
        {
            get
            {
                return Name + " asc ";
            }
        }
        /// <summary>
        /// 获取对象相应字段的值
        /// </summary>
        /// <param name="value">实体对象</param>
        /// <returns>字段对象</returns>
        public Field NewValue(object value)
        {

            return new Field { Name = Name, Value = Mappings.PropertyCastAttribute.CastValue(Table, Name, value) };
        }
        /// <summary>
        /// 获取对象在某区间的表达式
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>Expression</returns>
        public Expression this[T min, T max]
        {
            get
            {
                return Between(min, max);
            }
        }
       /// <summary>
       /// 获取基于些属性内嵌表达式
       /// </summary>
       /// <param name="exp">内嵌表达式</param>
        /// <returns>InTableExpression</returns>
        public InTableExpression this[Expression exp]
        {
            get
            {
                InTableExpression intbl = new InTableExpression();
                intbl.Field = this;
                intbl.Expression = exp;
                return intbl;
            }
        }
    }
}
