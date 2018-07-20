
using System;
using System.Collections.Generic;

namespace Common.Entity.Internal
{
    internal sealed class EntityMetadata
    {
        public EntityMetadata(Type type)
        {
            EntityType = type;
            LowerCaseKey = false; // 不能默认为小写，平台过去很多地方是默认的字段名称没有小写
            Propertys = new List<PropertyMetadata>();
            Fields = new List<PropertyMetadata>();
            _dbFieldNames = new List<string>();
        }

        /// <summary>
        /// 实体对应数据库表名
        /// </summary>
        public string TableName;

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType;

        /// <summary>
        /// 属性对应字段是否转为小写，默认是
        /// </summary>
        public bool LowerCaseKey;

        /// <summary>
        /// 属性集
        /// </summary>
        public List<PropertyMetadata> Propertys;

        /// <summary>
        /// 字段集
        /// </summary>
        public List<PropertyMetadata> Fields;


        private List<string> _dbFieldNames;
        /// <summary>
        /// 实体对应数据库字段集合
        /// </summary>
        public List<string> DbFieldNames
        {
            get
            {
                return this._dbFieldNames;
            }
        }

        /// <summary>
        /// 添加属性集
        /// </summary>
        public void AddProperty(PropertyMetadata field)
        {
            Propertys.Add(field);

            string fieldName = field.FieldName;
            if (!field.IsDbIgnore)
            {
                _dbFieldNames.Add(fieldName);
            }
        }

        public void AddFields(PropertyMetadata field)
        {
            Fields.Add(field);
        }

        internal PropertyMetadata GetPropertyMeta(string propertyName)
        {
            return Propertys.Find((meta) =>
            {
                if (LowerCaseKey)
                {
                    return meta.FieldName == propertyName.ToLower();
                }
                return meta.FieldName == propertyName;
            });
        }
    }
}
