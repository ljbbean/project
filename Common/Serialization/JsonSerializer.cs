
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using Common.Serialization;
using Common.Part;
using Common.Entity.Internal;
using Common.Script;

namespace Common
{
    /// <summary>
    /// 一般用于输出脚本
    /// </summary>
    internal class RawText
    {
        private string _value;

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="value"></param>
        public RawText(string value)
        {
            this._value = value;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }

    /// <summary>
    /// JSON 序列号
    /// </summary>
    public class JsonSerializer
    {
        internal const string ServerTypeFieldName = "__type";
        internal const int DefaultRecursionLimit = 100;
        private static int DefaultMaxJsonLength = 5 * 1024 * 1024; // 原来是 2M

        static JsonSerializer()
        {
        }

        /// <summary>
        /// 序列化对象为字符串
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializeInternal(object o)
        {
            return CreateInstance().Serialize(o);
        }

        internal static object Deserialize(JsonSerializer serializer, string input, Type type, int depthLimit)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (input.Length > serializer.MaxJsonLength)
            {
                throw new ArgumentException(Resources.JSON_MaxJsonLengthExceeded, "input");
            }

            object o = JsonDeserializer.BasicDeserialize(input, depthLimit, serializer);
            return ObjectConverter.ConvertObjectToType(o, type, serializer);
        }

        private JsonTypeResolver _typeResolver;
        private int _recursionLimit;
        private int _maxJsonLength;

        /// <summary>
        /// 用默认配置创建实例
        /// </summary>
        /// <returns></returns>
        public static JsonSerializer CreateInstance()
        {
            return CreateInstance(null);
        }

        /// <summary>
        /// 带类型解析
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static JsonSerializer CreateInstance(JsonTypeResolver resolver)
        {
            return new JsonSerializer(resolver);
        }

        private JsonSerializer()
            : this(null)
        {

        }

        private JsonSerializer(JsonTypeResolver resolver)
        {
            _typeResolver = resolver;
            RecursionLimit = DefaultRecursionLimit;
            MaxJsonLength = DefaultMaxJsonLength;
        }

        /// <summary>
        /// 最大结果长度
        /// </summary>
        public int MaxJsonLength
        {
            get
            {
                return _maxJsonLength;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value", Resources.JSON_InvalidMaxJsonLength);
                }
                _maxJsonLength = value;
            }
        }

        /// <summary>
        /// 最大递归层次
        /// </summary>
        public int RecursionLimit
        {
            get
            {
                return _recursionLimit;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value", Resources.JSON_InvalidRecursionLimit);
                }
                _recursionLimit = value;
            }
        }

        internal JsonTypeResolver TypeResolver
        {
            get
            {
                return _typeResolver;
            }
        }

        private Dictionary<Type, JsonConverter> _converters;
        private Dictionary<Type, JsonConverter> Converters
        {
            get
            {
                if (_converters == null)
                {
                    _converters = new Dictionary<Type, JsonConverter>();
                }
                return _converters;
            }
        }

        /// <summary>
        /// 注册转换器
        /// </summary>
        /// <param name="converters"></param>
        public void RegisterConverters(IEnumerable<JsonConverter> converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException("converters");
            }

            foreach (JsonConverter converter in converters)
            {
                IEnumerable<Type> supportedTypes = converter.SupportedTypes;
                if (supportedTypes != null)
                {
                    foreach (Type supportedType in supportedTypes)
                    {
                        Converters[supportedType] = converter;
                    }
                }
            }
        }

        private JsonConverter GetConverter(Type t)
        {
            if (_converters != null)
            {
                while (t != null)
                {
                    if (_converters.ContainsKey(t))
                    {
                        return _converters[t];
                    }
                    t = t.BaseType;
                }
            }
            return null;
        }

        internal bool ConverterExistsForType(Type t, out JsonConverter converter)
        {
            converter = GetConverter(t);
            return converter != null;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public object DeserializeObject(string input)
        {
            return Deserialize(this, input, null, RecursionLimit);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public T Deserialize<T>(string input)
        {
            return (T)Deserialize(this, input, typeof(T), RecursionLimit);
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public T ConvertToType<T>(object obj)
        {
            return (T)ConvertToType(obj, typeof(T));
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        public object ConvertToType(object obj, Type type)
        {
            return ObjectConverter.ConvertObjectToType(obj, type, this);
        }

        /// <summary>
        /// 序列化，默认是 JavaScript 格式，Atlas的默认的是 JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            return Serialize(obj, SerializationFormat.JavaScript);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="output"></param>
        public void Serialize(object obj, StringBuilder output)
        {
            Serialize(obj, output, SerializationFormat.JavaScript);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializationFormat"></param>
        /// <returns></returns>
        public string Serialize(object obj, SerializationFormat serializationFormat)
        {
            StringBuilder output = new StringBuilder();
            Serialize(obj, output, serializationFormat);
            return output.ToString();
        }

        internal void Serialize(object obj, StringBuilder output, SerializationFormat serializationFormat)
        {
            //lock (obj)// 并发有时会报错:集合已修改；可能无法执行枚举操作 ljb,2013.08.01
            {
                SerializeValue(obj, output, 0, null, serializationFormat);
            }
            if (output.Length > MaxJsonLength)
            {
                throw new InvalidOperationException(Resources.JSON_MaxJsonLengthExceeded);
            }
        }

        private static void SerializeBoolean(bool o, StringBuilder sb)
        {
            if (o)
            {
                sb.Append("true");
            }
            else
            {
                sb.Append("false");
            }
        }

        private static void SerializeUri(Uri uri, StringBuilder sb)
        {
            sb.Append("\"").Append(uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped)).Append("\"");
        }

        private static void SerializeGuid(Guid guid, StringBuilder sb)
        {
            sb.Append("\"").Append(guid.ToString()).Append("\"");
        }

        private static void SerializeCuid(ulong cuid, StringBuilder sb)
        {
            if (cuid == 0)
            {
                sb.Append("0"); // 数字的0，而不是字符串的"0"
            }
            else
            {
                sb.Append("\"").Append(cuid.ToString()).Append("\"");
            }
        }

        private static void SerializeCuidForSqlServer(long cuid, StringBuilder sb)
        {
            if (cuid == 0)
            {
                sb.Append("0"); // 数字的0，而不是字符串的"0"
            }
            else
            {
                sb.Append("\"").Append(cuid.ToString()).Append("\"");
            }
        }

        internal static readonly long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
        private static void SerializeDateTime(DateTime datetime, StringBuilder sb,
            SerializationFormat serializationFormat)
        {
            if (serializationFormat == SerializationFormat.JSON)
            {
                sb.Append("\"\\/Date(");
                sb.Append((long)(((datetime.ToUniversalTime()).Ticks - DatetimeMinTimeTicks) / 0x2710L));
                sb.Append(")\\/\"");
            }
            else
            {
                sb.Append("new Date(");
                sb.Append((long)(((datetime.ToUniversalTime()).Ticks - DatetimeMinTimeTicks) / 0x2710L));
                sb.Append(")");
            }
        }

        private void SerializeCustomObject(object o, StringBuilder sb, int depth, Hashtable objectsInUse,
            SerializationFormat serializationFormat)
        {
            bool first = true;
            Type type = o.GetType();
            sb.Append('{');

            if (TypeResolver != null)
            {
                string typeString = TypeResolver.ResolveTypeId(type);
                if (typeString != null)
                {
                    SerializeString(ServerTypeFieldName, sb);
                    sb.Append(':');
                    SerializeValue(typeString, sb, depth, objectsInUse, serializationFormat);
                    first = false;
                }
            }

            EntityMetadata typeData = EntityMetadataCache.GetEntityMetadata(type);// 使用统一缓存方式 add by zq 2015-12-30
            foreach (PropertyMetadata pMeta in typeData.Fields)
            {
                FieldInfo fieldInfo = pMeta.FieldInfo;
                if (fieldInfo.IsDefined(typeof(ScriptIgnoreAttribute), true)) continue;

                if (!first) sb.Append(',');
                SerializeString(pMeta.FieldName, sb);
                sb.Append(':');
                SerializeValue(fieldInfo.GetValue(o), sb, depth, objectsInUse, serializationFormat);
                first = false;
            }

            foreach (PropertyMetadata pMeta in typeData.Propertys)
            {
                PropertyInfo propInfo = pMeta.PropertyInfo;
                if (propInfo.IsDefined(typeof(ScriptIgnoreAttribute), true))
                    continue;

                MethodInfo getMethodInfo = propInfo.GetGetMethod();
                if (getMethodInfo == null)
                    continue;

                if (getMethodInfo.GetParameters().Length > 0) continue;

                if (!first) sb.Append(',');
                SerializeString(pMeta.FieldName, sb);
                sb.Append(':');
                SerializeValue(getMethodInfo.Invoke(o, null), sb, depth, objectsInUse, serializationFormat);
                first = false;
            }

            sb.Append('}');
        }

        private void SerializeDictionary(IDictionary o, StringBuilder sb, int depth, Hashtable objectsInUse,
            SerializationFormat serializationFormat)
        {
            sb.Append('{');
            bool isFirstElement = true;
            DictionaryEntry[] entrys = new DictionaryEntry[o.Count];
            o.CopyTo(entrys, 0);
            for (int i = 0; i < entrys.Length; i++)
            {
                DictionaryEntry entry = entrys[i];
                if (!isFirstElement)
                {
                    sb.Append(',');
                }
                string key = entry.Key as string;
                if (key == null)
                {
                    throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, Resources.JSON_DictionaryTypeNotSupported, o.GetType().FullName));
                }
                SerializeString((string)entry.Key, sb);
                sb.Append(':');
                SerializeValue(entry.Value, sb, depth, objectsInUse, serializationFormat);
                isFirstElement = false;

            }
            /* foreach (DictionaryEntry entry in (IDictionary)o)   // 并发有时会报错:集合已修改；可能无法执行枚举操作 msf,2012.2.28
              {
                  if (!isFirstElement)
                  {
                      sb.Append(',');
                  }
                  string key = entry.Key as string;
                  if (key == null)
                  {
                      throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, Resources.JSON_DictionaryTypeNotSupported, o.GetType().FullName));
                  }
                  SerializeString((string)entry.Key, sb);
                  sb.Append(':');
                  SerializeValue(entry.Value, sb, depth, objectsInUse, serializationFormat);
                  isFirstElement = false;
              }*/
            sb.Append('}');
        }

        private void SerializeEnumerable(IEnumerable enumerable, StringBuilder sb, int depth, Hashtable objectsInUse,
            SerializationFormat serializationFormat)
        {
            sb.Append('[');
            bool isFirstElement = true;

            List<object> list = new List<object>();
            foreach (object o in enumerable)
            {
                list.Add(o);
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (!isFirstElement)
                {
                    sb.Append(',');
                }

                SerializeValue(list[i], sb, depth, objectsInUse, serializationFormat);
                isFirstElement = false;
            }

            //foreach (object o in enumerable)
            //{
            //    if (!isFirstElement)
            //    {
            //        sb.Append(',');
            //    }

            //    SerializeValue(o, sb, depth, objectsInUse, serializationFormat);
            //    isFirstElement = false;
            //}

            sb.Append(']');
        }

        private static void SerializeString(string input, StringBuilder sb)
        {
            sb.Append('"');
            sb.Append(JsonString.QuoteString(input));
            sb.Append('"');
        }

        private void SerializeValue(object o, StringBuilder sb, int depth, Hashtable objectsInUse, SerializationFormat serializationFormat)
        {
            if (++depth > _recursionLimit)
            {
                throw new ArgumentException(Resources.JSON_DepthLimitExceeded);
            }

            JsonConverter converter = null;
            if (o != null && ConverterExistsForType(o.GetType(), out converter))
            {
                IDictionary<string, object> dict = converter.Serialize(o, this);

                if (TypeResolver != null)
                {
                    string typeString = TypeResolver.ResolveTypeId(o.GetType());
                    if (typeString != null)
                    {
                        dict[ServerTypeFieldName] = typeString;
                    }
                }

                sb.Append(Serialize(dict, serializationFormat));
                return;
            }

            // 内置对 DataTable 和 DataRow 的支持
            if (o is DataTable)
            {
                o = DataHelper.DataTableToObjectList((DataTable)o);
            }
            else if (o is DataRow)
            {
                o = DataHelper.DataRowToObject((DataRow)o);
            }
            // End 内置

            SerializeValueInternal(o, sb, depth, objectsInUse, serializationFormat);
        }

        private class ReferenceComparer : IEqualityComparer
        {
            bool IEqualityComparer.Equals(object x, object y)
            {
                return x == y;
            }

            int IEqualityComparer.GetHashCode(object obj)
            {
                return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
            }
        }

        private void SerializeValueInternal(object o, StringBuilder sb, int depth, Hashtable objectsInUse,
            SerializationFormat serializationFormat)
        {
            if (o == null || DBNull.Value.Equals(o))
            {
                sb.Append("null");
                return;
            }

            string os = o as String;
            if (os != null)
            {
                SerializeString(os, sb);
                return;
            }

            if (o is Char)
            {
                if ((char)o == '\0')
                {
                    sb.Append("null");
                    return;
                }
                SerializeString(o.ToString(), sb);
                return;
            }

            if (o is bool)
            {
                SerializeBoolean((bool)o, sb);
                return;
            }

            if (o is DateTime)
            {
                SerializeDateTime((DateTime)o, sb, serializationFormat);
                return;
            }

            if (o is TimeSpan)
            {
                SerializeDateTime(Convert.ToDateTime(o.ToString()), sb, serializationFormat); // 当前日期 + TimeSpan对应的时间
                return;
            }

            if (o is ulong)
            {
                SerializeCuid((ulong)o, sb);
                return;
            }

            if (o is long && serializationFormat == SerializationFormat.JavaScript) // Carpa3版本传给JavaScript时long也必须转成字符串
            {
                SerializeCuidForSqlServer((long)o, sb);
                return;
            }

            if (o is Guid)
            {
                SerializeGuid((Guid)o, sb);
                return;
            }

            if (o is RawText)
            {
                sb.Append(((RawText)o).Value);
                return;
            }

            Uri uri = o as Uri;
            if (uri != null)
            {
                SerializeUri(uri, sb);
                return;
            }

            if (o is double)
            {
                sb.Append(((double)o).ToString("r", CultureInfo.InvariantCulture));
                return;
            }

            if (o is float)
            {
                sb.Append(((float)o).ToString("r", CultureInfo.InvariantCulture));
                return;
            }

            if (o.GetType().IsPrimitive || o is Decimal)
            {
                IConvertible convertible = o as IConvertible;
                if (convertible != null)
                {
                    sb.Append(convertible.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    throw new InvalidOperationException();
                    //sb.Append(o.ToString());
                }
                return;
            }

            Type type = o.GetType();
            if (type.IsEnum)
            {
                sb.Append(Convert.ChangeType(o, (o as Enum).GetTypeCode()));
                return;
            }

            try
            {
                if (objectsInUse == null)
                {
                    objectsInUse = new Hashtable(new ReferenceComparer());
                }
                else if (objectsInUse.ContainsKey(o))
                {
                    throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, Resources.JSON_CircularReference, type.FullName));
                }

                objectsInUse.Add(o, null);

                IDictionary od = o as IDictionary;
                if (od != null)
                {
                    SerializeDictionary(od, sb, depth, objectsInUse, serializationFormat);
                    return;
                }

                IEnumerable oenum = o as IEnumerable;
                if (oenum != null)
                {
                    SerializeEnumerable(oenum, sb, depth, objectsInUse, serializationFormat);
                    return;
                }

                SerializeCustomObject(o, sb, depth, objectsInUse, serializationFormat);
            }
            finally
            {
                if (objectsInUse != null)
                {
                    objectsInUse.Remove(o);
                }
            }
        }

        /// <summary>
        /// 序列化格式
        /// </summary>
        public enum SerializationFormat
        {
            /// <summary>
            /// JSON 格式
            /// </summary>
            JSON,
            /// <summary>
            /// JavaScript 格式
            /// </summary>
            JavaScript
        }
    }


}