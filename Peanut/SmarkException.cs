using System;
using System.Collections.Generic;
using System.Text;

namespace Peanut
{
    /// <summary>
    /// 数据访问错误异常
    /// </summary>
    public class PeanutException:Exception
    {
        /// <summary>
        /// 构建异常
        /// </summary>
        public PeanutException() { }
        /// <summary>
        /// 构建异常,并指写具体的错误信息
        /// </summary>
        /// <param name="err">错误信息</param>
        public PeanutException(string err) : base(err) { }
        /// <summary>
        /// 构建异常,并指写具体的错误信息和内部异常类
        /// </summary>
        /// <param name="err">错误信息</param>
        /// <param name="baseexc">内部异常类</param>
        public PeanutException(string err, Exception baseexc) : base(err, baseexc) { }
    }
}
