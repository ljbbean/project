
using Common.Part;
using Common.Script;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common.Entity.Internal
{
    internal static class EntityMetadataCache
    {
        private static IDictionary<Type, EntityMetadata> cache = new Dictionary<Type, EntityMetadata>();

        /// <summary>
        /// 获取指定Type的静态属性集合
        /// </summary>
        /// <param name="entityType">Type对象</param>
        /// <returns></returns>
        public static IHashMap GetStaticEntityData(Type entityType)
        {
            IHashMap data = new HashMap();
            EntityMetadata metaData = EntityMetadataCache.GetEntityMetadata(entityType);
            foreach (PropertyMetadata field in metaData.Propertys)
            {
                PropertyInfo pInfo = field.PropertyInfo;
                if (!EntityFactory.IsPrimitiveType(field.PropertyInfo.PropertyType)) continue;  // 简单类型

                MethodInfo get = pInfo.GetGetMethod();
                if (get == null || !get.IsStatic) continue;     // 静态属性
                if (field.IsDbIgnore) continue;
                string name = field.FieldName;
                object value = pInfo.GetValue(null, null);
                data.Add(name, value);
            }
            return data;
        }

        public static EntityMetadata GetEntityMetadata(Type entityType)
        {
            EntityMetadata entityMetaData;
            if (cache.TryGetValue(entityType, out entityMetaData))
                return entityMetaData;

            lock (cache)
            {
                if (cache.TryGetValue(entityType, out entityMetaData))
                {
                    return entityMetaData;
                }

                entityMetaData = DoGetEntityType(entityType);
                cache.Add(entityType, entityMetaData);
            }
            return entityMetaData;
        }

        private static EntityMetadata DoGetEntityType(Type entityType)
        {
            EntityMetadata table = new EntityMetadata(entityType);

            string tableName = entityType.Name.ToLower();
            object[] attrs = entityType.GetCustomAttributes(typeof(EntityClassAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                EntityClassAttribute tableAttribute = (EntityClassAttribute)attrs[0];
                table.LowerCaseKey = tableAttribute.LowerCaseKey;
                if (!string.IsNullOrEmpty(tableAttribute.TableName))
                {
                    tableName = tableAttribute.TableName.ToLower();
                }
            }
            table.TableName = tableName;
            InitPropertieInfo(entityType, table);
            return table;
        }

        private static void InitPropertieInfo(Type entityType, EntityMetadata table)
        {
            foreach (PropertyInfo propertyInfo in entityType.GetProperties())
            {
                PropertyMetadata property = new PropertyMetadata();
                string fieldName = DealFieldName(propertyInfo, property, table);
                property.FieldName = fieldName;
                property.PropertyInfo = propertyInfo;
                table.AddProperty(property);
            }

            foreach (FieldInfo propertyInfo in entityType.GetFields())
            {
                PropertyMetadata property = new PropertyMetadata();
                string fieldName = DealFieldName(propertyInfo, property, table);
                property.FieldName = fieldName;
                property.FieldInfo = propertyInfo;
                table.AddFields(property);
            }
        }

        private static string DealFieldName(MemberInfo propertyInfo, PropertyMetadata property, EntityMetadata table)
        {
            string fieldName = propertyInfo.Name;
            object[] attributes = propertyInfo.GetCustomAttributes(typeof(FieldAttribute), true);
            if (attributes.Length > 0)
            {
                FieldAttribute fieldAttribute = (FieldAttribute)attributes[0];
                if (!string.IsNullOrEmpty(fieldAttribute.FieldName))
                {
                    fieldName = fieldAttribute.FieldName;
                }
                if (fieldAttribute is DbIgnoreAttribute)
                {
                    property.IsDbIgnore = true;
                }
            }
            return table.LowerCaseKey ? fieldName.ToLower() : fieldName;
        }
    }
}
