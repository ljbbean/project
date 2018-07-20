using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Part
{
    /// <summary>
    /// 字段定义
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        private string fieldName;

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="fieldName">为空是允许的，即无效，仍然等于属性名</param>
        public FieldAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
        }
    }
}
