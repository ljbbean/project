using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Reflection;
using Common.Script;
using Common.Part;
using System.Data;
using Common.Entity.Internal;

namespace Common.Entity
{
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T1"></typeparam>
    ///// <typeparam name="T2"></typeparam>
    ///// <typeparam name="TResult"></typeparam>
    ///// <param name="arg1"></param>
    ///// <param name="arg2"></param>
    ///// <returns></returns>
    //public delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T1"></typeparam>
    ///// <typeparam name="T2"></typeparam>
    ///// <param name="arg1"></param>
    ///// <param name="arg2"></param>
    //public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T1"></typeparam>
    ///// <typeparam name="T2"></typeparam>
    ///// <typeparam name="T3"></typeparam>
    ///// <param name="arg1"></param>
    ///// <param name="arg2"></param>
    ///// <param name="arg3"></param>
    //public delegate void Action<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);


    /// <summary>
    /// 实体类型扩展类
    /// </summary>
    public static class EntityExtension
    {
        /// <summary>
        /// 将HashObjectList转换为指定类型TEntity的对象列表
        /// </summary>
        /// <typeparam name="TEntity">将要转换的目标对象类型</typeparam>
        /// <param name="list">将要进行类型转换的数据源</param>
        /// <returns>指定类型为TEntity的对象列表</returns>
        public static IList<TEntity> ToEntities<TEntity>(this IHashMapList list) where TEntity : class,new()
        {
            if (list == null)
            {
                return null;
            }

            IList<TEntity> entities = new List<TEntity>();
            foreach (IHashMap ho in list)
            {
                entities.Add(ToEntity<TEntity>(ho));
            }
            return entities;
        }

        /// <summary>
        /// 将HashObject转换为指定类型TEntity的对象
        /// </summary>
        /// <typeparam name="TEntity">将要转换的目标对象类型</typeparam>
        /// <param name="data">将要进行类型转换的数据源</param>
        /// <returns>指定类型为TEntity的对象</returns>
        public static TEntity ToEntity<TEntity>(this IHashMap data) where TEntity : class,new()
        {
            return EntityFactory.GetEntity<TEntity, IHashMap>(data, (ho, key) =>
            {
                // 这种方式数据较多的情况下性能比较差
                if (ho.ContainsKey(key))
                {
                    return ho.Get<object>(key);
                }
                return null;


                /* 部分机器性能有问题，先还原回去
                try
                {
                    return ho[key];
                }
                catch
                {
                    Logging.Log.Info("error.............");
                    return null;
                }
                 */
            });
        }

        /// <summary>
        /// 将DataTable转换为指定类型TEntity的对象列表
        /// </summary>
        /// <typeparam name="TEntity">将要转换的目标对象类型</typeparam>
        /// <param name="table">将要进行类型转换的数据源</param>
        /// <returns>指定类型为TEntity的对象列表</returns>
        public static IList<TEntity> ToEntities<TEntity>(this DataTable table) where TEntity : class,new()
        {
            if (table == null) return null;
            return ToEntities<TEntity>(table.Rows);
        }

        /// <summary>
        /// 将DataRowCollection转换为指定类型TEntity的对象列表
        /// </summary>
        /// <typeparam name="TEntity">将要转换的目标对象类型</typeparam>
        /// <param name="rows">将要进行类型转换的数据源</param>
        /// <returns>指定类型为TEntity的对象列表</returns>
        public static IList<TEntity> ToEntities<TEntity>(this DataRowCollection rows) where TEntity : class,new()
        {
            if (rows == null) return null;

            IList<TEntity> entities = new List<TEntity>();
            foreach (DataRow dr in rows)
            {
                entities.Add(ToEntity<TEntity>(dr));
            }
            return entities;
        }

        /// <summary>
        /// 将DataRow转换为指定类型TEntity的对象
        /// </summary>
        /// <typeparam name="TEntity">将要转换的目标对象类型</typeparam>
        /// <param name="data">将要进行类型转换的数据源</param>
        /// <returns>指定类型为TEntity的对象</returns>
        public static TEntity ToEntity<TEntity>(this DataRow data) where TEntity : class,new()
        {
            return EntityFactory.GetEntity<TEntity, DataRow>(data, (dr, fieldName) =>
            {
                // 这种方式数据较多的情况下性能比较差
                if (dr.Table.Columns.Contains(fieldName))
                {
                    return dr[fieldName];
                }
                return null;

                //try
                //{
                //    return dr[fieldName];
                //}
                //catch
                //{
                //    return null;
                //}
            });
        }

        /// <summary>
        /// 将HashObjectList转换为指定类型TEntity的对象列表
        /// </summary>
        /// <typeparam name="TEntity">泛型参数，将要转换的目标对象类型</typeparam>
        /// <param name="list">将要进行类型转换的数据源（其枚举内容必须为HashObject实例）</param>
        /// <returns>指定类型为TEntity的对象列表</returns>
        public static IList<TEntity> GetEntities<TEntity>(IEnumerable list) where TEntity : class,new()
        {
            return EntityFactory.GetEntities<TEntity, IHashMap>(list, (ho, key) =>
            {
                // 这种方式数据较多的情况下性能比较差
                if (ho.ContainsKey(key))
                {
                    return ho.Get<object>(key);
                }
                return null;

                /*部分机器性能有问题，先还原回去
                 try
                 {
                     return ho[key];
                 }
                 catch
                 {
                     return null;
                 }
                 */
            });
        }

        /// <summary>
        /// 将Entity对象转换为IHashObject
        /// </summary>
        /// <typeparam name="TEntity">源数据类型</typeparam>
        /// <param name="entity">源数据</param>
        /// <returns>IHashObject</returns>
        public static IHashMap GetHashObjectOfEntity<TEntity>(TEntity entity) where TEntity : class
        {
            return EntityFactory.GetEntityData<TEntity, HashMap>(entity, (ho, key, value) =>
            {
                ho[key] = value;
            });
        }

        /// <summary>
        /// 实体列表转HashObject列表
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IList<IHashMap> GetHashObjectsOfEntities<TEntity>(IList<TEntity> entities) where TEntity : class
        {
            IList<HashMap> list = EntityFactory.GetEntityDatas<TEntity, HashMap>(entities, (ho, key, value) => ho[key] = value);

            if (list == null)
            {
                return null;
            }

            IList<IHashMap> tempList = new List<IHashMap>();
            foreach (HashMap item in list)
            {
                tempList.Add(item);
            }
            return tempList;
        }

        /// <summary>
        /// 实体列表转DataTable
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entitys">实体集合</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTableOfEntitys<TEntity>(IList<TEntity> entitys) where TEntity : class
        {
            if (entitys == null || entitys.Count < 1)
            {
                return new DataTable();
            }

            // 取实体缓存
            Type type = typeof(TEntity);
            EntityMetadata tableMeta = EntityMetadataCache.GetEntityMetadata(type);

            // 无需缓存表结构
            DataTable dt = new DataTable(tableMeta.TableName);
            tableMeta.Propertys.ForEach((pi) =>
            {
                Type columnType = pi.PropertyInfo.PropertyType;
                if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    columnType = columnType.GetGenericArguments()[0];
                }
                dt.Columns.Add(new DataColumn(pi.FieldName, columnType));
            });

            // 填充数据
            foreach (TEntity entity in entitys)
            {
                DataRow row = dt.NewRow();
                tableMeta.Propertys.ForEach((p) =>
                {
                    object value = p.PropertyInfo.GetValue(entity, null);
                    if (value == null) value = DBNull.Value;
                    row[p.FieldName] = value;
                });
                dt.Rows.Add(row);
            }
            return dt;
        }
    }

    /// <summary>
    /// 实体类型转换工厂
    /// </summary>
    public static class EntityFactory
    {
        private static Action<MethodInfo, object, object> setMethodAction = (method, obj, value) => { method.Invoke(obj, new Object[] { value }); };
        //private static Action<FieldInfo, object, object> setFieldAction = (field, obj, value) => { field.SetValue(obj, value); };

        /// <summary>
        /// 将源数据中转换为实体类
        /// </summary>
        /// <typeparam name="TEntity">泛型参数，实体类型</typeparam>
        /// <typeparam name="TData">泛型参数，源数据类型</typeparam>
        /// <param name="data">源数据</param>
        /// <param name="func">从源数据中取值的委托</param>
        /// <returns>实体类型</returns>
        public static TEntity GetEntity<TEntity, TData>(TData data, Func<TData, string, object> func)
            where TEntity : class,new()
            where TData : class
        {
            if (data == null)
            {
                return default(TEntity);
            }

            return (TEntity)GetEntity(data, typeof(TEntity), func);
        }

        /// <summary>
        /// 将源数据中转换为实体类
        /// </summary>
        /// <typeparam name="TEntity">泛型参数，实体类型</typeparam>
        /// <typeparam name="TData">泛型参数，源数据类型</typeparam>
        /// <param name="datas">源数据列表</param>
        /// <param name="func">从源数据中取值的委托</param>
        /// <returns>实体类型列表</returns>
        public static IList<TEntity> GetEntities<TEntity, TData>(IList<TData> datas, Func<TData, string, object> func)
            where TEntity : class,new()
            where TData : class
        {
            if (datas == null)
            {
                return null;
            }
            return GetEntities(datas, typeof(TEntity), func) as IList<TEntity>;
        }

        /// <summary>
        /// 将源数据中转换为实体类
        /// </summary>
        /// <typeparam name="TEntity">泛型参数，实体类型</typeparam>
        /// <typeparam name="TData">泛型参数，源数据类型</typeparam>
        /// <param name="datas">源数据列表，枚举对象必须为TData</param>
        /// <param name="func">从源数据中取值的委托</param>
        /// <returns>实体类型列表</returns>
        public static IList<TEntity> GetEntities<TEntity, TData>(IEnumerable datas, Func<TData, string, object> func)
            where TEntity : class,new()
            where TData : class
        {
            if (datas == null)
            {
                return null;
            }
            return GetEntities(datas, typeof(TEntity), func) as IList<TEntity>;
        }

        private static object GetEntity<TData>(TData data, Type entityType, Func<TData, string, object> getValueAction)
            where TData : class
        {
            if (data == null)
            {
                return null;
            }

            object entity = Activator.CreateInstance(entityType);                                               // 类型entityType要求new()约束
            EntityMetadata entityMeta = EntityMetadataCache.GetEntityMetadata(entityType);

            // 处理属性
            foreach (PropertyMetadata field in entityMeta.Propertys)
            {
                MethodInfo mInfo = field.PropertyInfo.GetSetMethod(); ;
                if (mInfo == null) continue;                                                                    // 无Setter的时候忽略此Property

                AssignToPropertyOrField<TData, MethodInfo>(data, entityType, getValueAction, setMethodAction, mInfo, field.FieldName, entity, field.PropertyInfo.PropertyType);
            }

            return entity;
        }

        private static void AssignToPropertyOrField<TData, TMethod>(TData data, Type entityType, Func<TData, string, object> getValueAction,
            Action<TMethod, object, object> setValueAction, TMethod mi, string key, object entity, Type propType) where TData : class
        {
            object value = getValueAction(data, key);                                                       // 执行委托获取值
            if (value == null || value is DBNull) return;                                                   // null或数据中没包含的情况使用默认值
            if (value.GetType() == propType || propType == typeof(object))                                  // 引用类型或值类型相同时,可直接赋值
            {
                setValueAction(mi, entity, value);
                return;
            }

            try
            {
                value = GetValue(value, propType);
            }
            catch (Exception ex)
            {
                throw new InvalidExpressionException(string.Format("实体类{0}字段{1}转换失败:{2}", entityType.FullName, key, ex.ToString())); // 业务实体和hash类型不匹配时报错，但异常不明确
            }

            //为了解决NullAble<XXX>类型的转换问题 2015-03-26 by jf
            if (IsPrimitiveType(propType) || propType.IsEnum)
            {
                setValueAction(mi, entity, value);
                return;
            }

            if (propType.IsGenericType)
            {
                if (IsIList(propType))
                {
                    Type genericType = propType.GetGenericArguments()[0];
                    setValueAction(mi, entity, GetEntities<TData>(value as IEnumerable, genericType, getValueAction));
                }
                return; //IList<T>之外的泛型属性暂不处理
            }
            setValueAction(mi, entity, GetEntity(value as TData, propType, getValueAction));
        }

        private static IList GetEntities<TData>(IEnumerable datas, Type type, Func<TData, string, object> func)
            where TData : class
        {
            if (datas == null)
            {
                return null;
            }

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(type);
            IList entities = Activator.CreateInstance(constructedListType) as IList;
            foreach (object data in datas)
            {
                entities.Add(GetEntity(data as TData, type, func));
            }
            return entities;
        }

        /// <summary>
        /// 获取将实体类的数据
        /// </summary>
        /// <typeparam name="TEntity">泛型参数，实体类型</typeparam>
        /// <typeparam name="TData">泛型参数，数据类型</typeparam>
        /// <param name="entity">实体类实例</param>
        /// <param name="action">将数据保存到数据源的方式</param>
        /// <returns>
        /// 数据
        /// </returns>
        public static TData GetEntityData<TEntity, TData>(TEntity entity, Action<TData, string, object> action)
            where TEntity : class
            where TData : class,new()
        {
            if (entity == null)
            {
                return null;
            }
            return GetEntityData(entity, entity.GetType(), action);
        }

        /// <summary>
        /// 获取将实体类的数据
        /// </summary>
        /// <typeparam name="TEntity">泛型参数，实体类型</typeparam>
        /// <typeparam name="TData">泛型参数，数据类型</typeparam>
        /// <param name="entites">实体类实例列表</param>
        /// <param name="action">将数据保存到数据源的方式</param>
        /// <returns>
        /// 数据列表
        /// </returns>
        public static IList<TData> GetEntityDatas<TEntity, TData>(IList<TEntity> entites, Action<TData, string, object> action)
            where TEntity : class
            where TData : class,new()
        {
            if (entites == null)
            {
                return null;
            }
            return GetEntityDatas<TData>(entites, typeof(TEntity), action);
        }

        private static TData GetEntityData<TData>(object entity, Type entityType, Action<TData, string, object> action)
            where TData : class,new()
        {
            if (entity == null)
            {
                return null;
            }

            TData data = Activator.CreateInstance<TData>();//类型TData要求new()约束

            EntityMetadata entityMeta = EntityMetadataCache.GetEntityMetadata(entityType);
            foreach (PropertyMetadata field in entityMeta.Propertys)
            {
                MethodInfo mi = field.PropertyInfo.GetGetMethod();
                if (mi == null) continue;                               // 无Getter的时候忽略此Property

                string key = field.FieldName;
                object value = mi.Invoke(entity, null);

                if (value == null)
                {
                    action(data, key, value);
                    continue;
                }

                Type propType = field.PropertyInfo.PropertyType;
                if (propType.IsGenericType)
                {
                    if (propType.IsValueType)
                    {
                        action(data, key, value);
                    }
                    else if (IsIList(propType))//TODO：未处理循环引用情况，此时会导致死循环
                    {
                        Type genericType = propType.GetGenericArguments()[0];
                        if (genericType.IsClass)
                        {
                            IList entites = value as IList;
                            IList<TData> list = GetEntityDatas<TData>(entites, genericType, action);
                            action(data, key, list);
                        }
                    }
                    continue;
                }

                if (IsPrimitiveType(propType) || propType.IsEnum)
                {
                    action(data, key, value);
                    continue;
                }

                action(data, key, GetEntityData(value, propType, action));
            }
            return data;
        }


        private static IList<TData> GetEntityDatas<TData>(IEnumerable entites, Type entityType, Action<TData, string, object> action)
            where TData : class,new()
        {
            if (entites == null)
            {
                return null;
            }

            IList<TData> list = new List<TData>();
            foreach (object objEn in entites)
            {
                TData item = GetEntityData<TData>(objEn, entityType, action);
                list.Add(item);
            }
            return list;
        }

        private static bool IsIList(Type propType)
        {
            return typeof(IList<>) == propType.GetGenericTypeDefinition() || typeof(List<>) == propType.GetGenericTypeDefinition();
        }

        internal static object GetValue(object value, Type type)
        {
            Type underlyingType = type;
            if (value != null && ReflectionUtils.IsPrimitiveType(type, out underlyingType))
            {
                return ReflectionUtils.ChangeType(value, underlyingType); // 字符串转Int32报错
            }
            else if (value != null &&
                underlyingType.IsEnum && !value.GetType().IsEnum) // 整数或short等转成 Nullable<enum> 或 enum
            {
                return Enum.ToObject(underlyingType, value);
            }
            else if (value == null && underlyingType.IsSubclassOf(typeof(ValueType)))
            {
                return GetDefaultValue(type); // null转成ulong
            }
            else // 只能强转，可能出错
            {
                return value;
            }
        }

        internal static bool IsPrimitiveType(Type type)
        {
            Type underlyingType;
            return ReflectionUtils.IsPrimitiveType(type, out underlyingType);
        }

        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}