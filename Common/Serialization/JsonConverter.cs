using System;
using System.Collections.Generic;

namespace Common.Serialization
{
    /// <summary>
    /// ����ת���ӿ�
    /// </summary>
    public abstract class JsonConverter
    {
        /// <summary>
        /// ֧�ֵ�����
        /// </summary>
        public abstract IEnumerable<Type> SupportedTypes
        {
            get;
        }

        /// <summary>
        /// �����к�
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public abstract object Deserialize(IDictionary<string, object> dictionary, Type type, JsonSerializer serializer);
        /// <summary>
        /// ���к�
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public abstract IDictionary<string, object> Serialize(object obj, JsonSerializer serializer);
    }
}