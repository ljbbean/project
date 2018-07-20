using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Part
{
    /// <summary>
    /// 标识提交时忽略的字段
    /// </summary>
    public sealed class DbIgnoreAttribute : FieldAttribute
    {
        /// <summary>
        /// 构建函数
        /// </summary>
        public DbIgnoreAttribute()
            : base(null)
        {
        }

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="fieldName">为空是允许的，即无效，仍然等于属性名</param>
        public DbIgnoreAttribute(string fieldName)
            : base(fieldName)
        {
        }
    }
}
