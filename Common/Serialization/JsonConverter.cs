using System;
using System.Collections.Generic;

namespace Common.Serialization
{
    /// <summary>
    /// 类型转换接口
    /// </summary>
    public abstract class JsonConverter
    {
        /// <summary>
        /// 支持的类型
        /// </summary>
        public abstract IEnumerable<Type> SupportedTypes
        {
            get;
        }

        /// <summary>
        /// 反序列号
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public abstract object Deserialize(IDictionary<string, object> dictionary, Type type, JsonSerializer serializer);
        /// <summary>
        /// 序列号
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public abstract IDictionary<string, object> Serialize(object obj, JsonSerializer serializer);
    }
}