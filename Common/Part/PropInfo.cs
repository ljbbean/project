using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Part
{

    /// <summary>
    /// 代表 PropertyInfo 或者 FieldInfo
    /// </summary>
    public sealed class PropInfo
    {
        private object obj;
        private MemberInfo member;
        private PropertyInfo prop;
        private FieldInfo field;

        private PropInfo(object obj, MemberInfo member)
        {
            this.obj = obj;
            this.member = member;
        }

        internal PropInfo(object obj, PropertyInfo prop)
            : this(obj, (MemberInfo)prop)
        {
            this.prop = prop;
        }

        internal PropInfo(object obj, FieldInfo field)
            : this(obj, (MemberInfo)field)
        {
            this.field = field;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return member.Name; }
        }

        /// <summary>
        /// 值
        /// </summary>
        public object Value
        {
            get
            {
                if (prop != null)
                {
                    return prop.GetValue(obj, null);
                }
                else if (field != null)
                {
                    return field.GetValue(obj);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            set
            {
                if (prop != null)
                {
                    prop.SetValue(obj, value, null);
                }
                else if (field != null)
                {
                    field.SetValue(obj, value);
                }
            }
        }

        /// <summary>
        /// 类
        /// </summary>
        public Type Type
        {
            get { return prop != null ? prop.PropertyType : field.FieldType; }
        }
    }
}
