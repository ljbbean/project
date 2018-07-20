
using Common.Entity.Internal;
using Common.Part;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Common.Serialization
{
    internal static class ObjectConverter
    {
        private static readonly Type[] s_emptyTypeArray = new Type[] { };
        private static Type _listGenericType = typeof(List<>);
        private static Type _enumerableGenericType = typeof(IEnumerable<>);
        private static Type _dictionaryGenericType = typeof(Dictionary<,>);
        private static Type _idictionaryGenericType = typeof(IDictionary<,>);

        internal static bool IsClientInstantiatableType(Type t, JsonSerializer serializer)
        {
            if ((t == null || t.IsAbstract) || (t.IsInterface || t.IsArray))
                return false;

            if (t == typeof(object))
                return false;

            JsonConverter converter = null;
            if (serializer.ConverterExistsForType(t, out converter))
            {
                return true;
            }

            if (t.IsValueType)
            {
                return true;
            }

            ConstructorInfo constructorInfo = t.GetConstructor(BindingFlags.Public | BindingFlags.Instance,
                null, s_emptyTypeArray, null);
            if (constructorInfo == null)
                return false;

            return true;
        }

        private static void AddItemToList(IList oldList, IList newList, Type elementType, JsonSerializer serializer)
        {
            foreach (Object propertyValue in oldList)
            {
                newList.Add(ConvertObjectToType(propertyValue, elementType, serializer));
            }
        }

        private static void AssignToPropertyOrField(object propertyValue, object o, string memberName, JsonSerializer serializer)
        {
            IDictionary dictionary = o as IDictionary;

            if (dictionary != null)
            {
                propertyValue = ConvertObjectToType(propertyValue, null, serializer);
                dictionary[memberName] = propertyValue;
                return;
            }

            Type serverType = o.GetType();
            EntityMetadata typeData = EntityMetadataCache.GetEntityMetadata(serverType); // 使用缓存性能更好 add by zq 2015-12-30
            PropertyMetadata pMeta = typeData.Propertys.Find(delegate(PropertyMetadata meta) { return meta.FieldName.ToLower() == memberName.ToLower(); });

            if (pMeta != null)
            {
                PropertyInfo propInfo = pMeta.PropertyInfo;
                MethodInfo setter = propInfo.GetSetMethod();
                if (setter != null)
                {
                    propertyValue = ConvertObjectToType(propertyValue, propInfo.PropertyType, serializer, memberName);
                    if (typeof(IParamPropertySetter).IsAssignableFrom(serverType))
                    {
                        ((IParamPropertySetter)o).PropertySetterBeforeInvoke(propInfo.Name, propertyValue);
                    }
                    setter.Invoke(o, new Object[] { propertyValue });
                    return;
                }

            }

            pMeta = typeData.Fields.Find(delegate(PropertyMetadata meta) { return meta.FieldName.ToLower() == memberName.ToLower(); });
            if (pMeta != null)
            {
                FieldInfo fieldInfo = pMeta.FieldInfo;
                propertyValue = ConvertObjectToType(propertyValue, fieldInfo.FieldType, serializer, memberName);
                fieldInfo.SetValue(o, propertyValue);
                return;
            }
        }

        private static object ConvertDictionaryToObject(IDictionary<string, object> dictionary, Type type, JsonSerializer serializer)
        {
            Type targetType = type;
            object s;
            string serverTypeName = null;
            object o = dictionary;

            if (dictionary.TryGetValue(JsonSerializer.ServerTypeFieldName, out s))
            {
                serverTypeName = ConvertObjectToType(s, typeof(String), serializer) as String;
                if (serverTypeName != null)
                {
                    if (serializer.TypeResolver != null)
                    {
                        targetType = serializer.TypeResolver.ResolveType(serverTypeName);

                        if (targetType == null)
                        {
                            throw new InvalidOperationException();
                        }
                    }

                    dictionary.Remove(JsonSerializer.ServerTypeFieldName);
                }
            }

            JsonConverter converter = null;
            if (targetType != null && serializer.ConverterExistsForType(targetType, out converter))
            {
                return converter.Deserialize(dictionary, targetType, serializer);
            }

            if (serverTypeName != null ||
                (targetType != null && IsClientInstantiatableType(targetType, serializer)))
            {
                o = Activator.CreateInstance(targetType);
            }

#if INDIGO
			StructuralContract contract = null;
			if (suggestedType != null && 
                suggestedType.GetCustomAttributes(typeof(DataContractAttribute), false).Length > 0)
				contract = StructuralContract.Create(suggestedType);
#endif

            List<String> memberNames = new List<String>(dictionary.Keys);

            if (type != null && type.IsGenericType && (typeof(IDictionary).IsAssignableFrom(type) || type.GetGenericTypeDefinition() == _idictionaryGenericType) &&
                type.GetGenericArguments().Length == 2)
            {
                Type keyType = type.GetGenericArguments()[0];
                if (keyType != typeof(string) && keyType != typeof(object))
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, Resources.JSON_DictionaryTypeNotSupported, type.FullName));
                }

                Type valueType = type.GetGenericArguments()[1];
                IDictionary dict = null;
                if (IsClientInstantiatableType(type, serializer))
                {
                    dict = (IDictionary)Activator.CreateInstance(type);
                }
                else
                {
                    Type t = _dictionaryGenericType.MakeGenericType(keyType, valueType);
                    dict = (IDictionary)Activator.CreateInstance(t);
                }

                if (dict != null)
                {
                    foreach (string memberName in memberNames)
                    {
                        dict[memberName] = ConvertObjectToType(dictionary[memberName], valueType, serializer);
                    }
                    return dict;
                }
            }

            if (type != null && !type.IsAssignableFrom(o.GetType()))
            {
                ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, s_emptyTypeArray, null);
                if (constructorInfo == null)
                {
                    throw new MissingMethodException(String.Format(CultureInfo.InvariantCulture, Resources.JSON_NoConstructor, type.FullName));
                }

                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, Resources.JSON_DeserializerTypeMismatch, type.FullName));
            }

            foreach (string memberName in memberNames)
            {
                object propertyValue = dictionary[memberName];
#if INDIGO
	            if (contract != null) {
		            Member member = contract.FindMember(memberName);
		            
		            
		            if (member == null)
			            throw new InvalidOperationException();

		            if (member.MemberType == MemberTypes.Field) {
			            member.SetValue(o, propertyValue);
		            }
		            else {
			            member.SetValue(o, propertyValue);
		            }

                    continue;
	            }
#endif
                AssignToPropertyOrField(propertyValue, o, memberName, serializer);
            }

            return o;
        }

        internal static object ConvertObjectToType(object o, Type type, JsonSerializer serializer, string memberName = "")
        {
            if (o == null)
            {
                if (type == typeof(char))
                {
                    return '\0';
                }

                if (type != null && type.IsValueType && !(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    return Activator.CreateInstance(type); // 返回该类型的默认值 add by zq 2015-12-28
                    //throw new InvalidOperationException(string.Format(Resources.JSON_ValueTypeCannotBeNull, memberName)); // 值类型为null时，抛出的异常应提示出具体属性名称
                }
                return null;
            }

            if (o.GetType() == type)
            {
                return o;
            }

            try
            {
                return ConvertObjectToTypeInternal(o, type, serializer);
            }
            catch (Exception ex)
            {
                // 告诉业务具体字段转换失败，必须调整数据结构
                string msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += ex.InnerException.Message;
                }
                throw new TargetInvocationException(string.Format("'{0}' : {1}", memberName, msg), ex);
            }
        }

        private static object ConvertObjectToTypeInternal(object o, Type type, JsonSerializer serializer)
        {
            IDictionary<string, object> dictionary = o as IDictionary<string, object>;
            if (dictionary != null)
            {
                return ConvertDictionaryToObject(dictionary, type, serializer);
            }

            IList list = o as IList;
            if (list != null)
            {
                return ConvertListToObject(list, type, serializer);
            }

            if (type == null || o.GetType() == type)
            {
                return o;
            }

            try
            {
                return Convert.ChangeType(o, type); // 直接强转，add by zq 2015-12-30
            }
            catch (Exception)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);

                if (converter.CanConvertFrom(o.GetType()))
                {
                    return converter.ConvertFrom(null, CultureInfo.InvariantCulture, o);
                }

                if (converter.CanConvertFrom(typeof(String)))
                {
                    TypeConverter propertyConverter = TypeDescriptor.GetConverter(o);
                    string s = propertyConverter.ConvertToInvariantString(o);

                    return converter.ConvertFromInvariantString(s); // 该情况下 bool 转 int32会报错 add by zq 2015-12-30
                }

                if (type.IsAssignableFrom(o.GetType()))
                {
                    return o;
                }
            }

            throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.JSON_CannotConvertObjectToType, o.GetType(), type));
        }

        private static IList ConvertListToObject(IList list, Type type, JsonSerializer serializer)
        {
            if (type == null || type == typeof(Object) || type.IsArray || type == typeof(ArrayList)
                || type == typeof(IEnumerable) || type == typeof(IList) || type == typeof(ICollection))
            {
                Type elementType = typeof(Object);
                if (type != null && type != typeof(Object))
                {
                    elementType = type.GetElementType();
                }

                ArrayList newList = new ArrayList();

                AddItemToList(list, newList, elementType, serializer);

                if (type == typeof(ArrayList) || type == typeof(IEnumerable) || type == typeof(IList) || type == typeof(ICollection))
                {
                    return newList;
                }

                return newList.ToArray(elementType);
            }
            else if (type.IsGenericType &&
                type.GetGenericArguments().Length == 1)
            {
                Type elementType = type.GetGenericArguments()[0];

                Type strongTypedEnumerable = _enumerableGenericType.MakeGenericType(elementType);

                if (strongTypedEnumerable.IsAssignableFrom(type))
                {
                    Type t = _listGenericType.MakeGenericType(elementType);

                    IList newList = null;
                    if (IsClientInstantiatableType(type, serializer) && typeof(IList).IsAssignableFrom(type))
                    {
                        newList = (IList)Activator.CreateInstance(type);
                    }
                    else
                    {
                        if (t.IsAssignableFrom(type))
                        {
                            throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, Resources.JSON_CannotCreateListType, type.FullName));
                        }
                        newList = (IList)Activator.CreateInstance(t);
                    }

                    AddItemToList(list, newList, elementType, serializer);

                    return newList;
                }
            }
            else if (IsClientInstantiatableType(type, serializer) && typeof(IList).IsAssignableFrom(type))
            {
                IList newList = (IList)Activator.CreateInstance(type);

                AddItemToList(list, newList, null, serializer);

                return newList;
            }

            throw new InvalidOperationException(String.Format(
                CultureInfo.CurrentCulture, Resources.JSON_ArrayTypeNotSupported, type.FullName));
        }
    }
}
