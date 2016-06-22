using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peanut
{
    /// <summary>
    /// 关联表描述
    /// </summary>
    public class JoinTable
    {
        static string[] Jions = new string[] {"Left Join","Right Join","Inner Join","Outer Join" };
        /// <summary>
        /// 构建关联表
        /// </summary>
        /// <param name="left">左表字段</param>
        /// <param name="right">右表字段</param>
        /// <param name="type">关联类型</param>
        public JoinTable(IFieldInfo left, IFieldInfo right,JoinType type)
        {
            mTable.AppendFormat(" {0} {1} {2} on {0}.{3}={2}.{4} ",
                left.Table, Jions[(int)type], right.Table, left.Name, right.Name);
        }

        private StringBuilder mTable = new StringBuilder();

        private StringBuilder mFields = new StringBuilder();

        private void Join(IFieldInfo left, IFieldInfo right, JoinType type)
        {
            mTable.AppendFormat(" {0} {1} on {2}.{3}={1}.{4}", Jions[(int)type], right.Table,
                left.Table, left.Name, right.Name);
        }
        /// <summary>
        /// 左联到指定的表字段
        /// </summary>
        /// <param name="left">左表字段</param>
        /// <param name="right">右表字段</param>
        /// <returns>JoinTable</returns>
        public JoinTable LeftJoin(IFieldInfo left, IFieldInfo right)
        {
            Join(left, right, JoinType.Left);
            return this;
        }
        /// <summary>
        /// 右联到指定的表字段
        /// </summary>
        /// <param name="left">左表字段</param>
        /// <param name="right">右表字段</param>
        /// <returns>JoinTable</returns>
        public JoinTable RightJoin(IFieldInfo left, IFieldInfo right)
        {
            Join(left, right, JoinType.Right);
            return this;
        }
        /// <summary>
        /// 内联到指定的表字段
        /// </summary>
        /// <param name="left">左表字段</param>
        /// <param name="right">右表字段</param>
        /// <returns>JoinTable</returns>
        public JoinTable InnerJoin(IFieldInfo left, IFieldInfo right)
        {
            Join(left, right, JoinType.Inner);
            return this;
        }
        /// <summary>
        /// 外联到指定的表字段
        /// </summary>
        /// <param name="left">左表字段</param>
        /// <param name="right">右表字段</param>
        /// <returns>JoinTable</returns>
        public JoinTable OuterJoin(IFieldInfo left, IFieldInfo right)
        {
            Join(left, right, JoinType.Outer);
            return this;
        }
        /// <summary>
        /// 获取相应的字段描述
        /// </summary>
        /// <param name="fields">字段列表</param>
        /// <returns>JoinTable</returns>
        public JoinTable Select(params IFieldInfo[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (mFields.Length > 0)
                    mFields.Append(",");
                mFields.Append(fields[i].Name);
            }
            return this;
        }
        /// <summary>
        /// 获取相应的字段描述
        /// </summary>
        /// <param name="fields">字段列表</param>
        /// <returns>JoinTable</returns>
        public JoinTable Select(params string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (mFields.Length > 0)
                    mFields.Append(",");
                mFields.Append(fields[i]);
            }
            return this;
        }

        public override string ToString()
        {
            return "select " + mFields.ToString() + " from " + mTable.ToString();
        }
    }
    /// <summary>
    /// 关联类型
    /// </summary>
    public enum JoinType:int
    {
        /// <summary>
        /// 左联
        /// </summary>
        Left=0,
        /// <summary>
        /// 右联
        /// </summary>
        Right=1,
        /// <summary>
        /// 内联
        /// </summary>
        Inner=2,
        /// <summary>
        /// 外联
        /// </summary>
        Outer=3
    }
}
