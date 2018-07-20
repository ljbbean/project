
using System;

namespace Common.Part
{
    /// <summary>
    /// 标记属性值不输出
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ScriptIgnoreAttribute : Attribute
    {
        /// <summary>
        /// 默认
        /// </summary>
        public ScriptIgnoreAttribute()
        {
        }
    }

    /// <summary>
    /// 实体类属性（字段名小写）
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityClassAttribute : Attribute
    {
        private bool _lowerCaseKey = true;
        private string _tableName;

        /// <summary>
        /// 
        /// </summary>
        public EntityClassAttribute()
        {
        }

        /// <summary>
        /// 字段或属性是否小写
        /// </summary>
        public bool LowerCaseKey
        {
            get { return _lowerCaseKey; }
            set { _lowerCaseKey = value; }
        }


        /// <summary>
        /// 数据表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }
    }
}
