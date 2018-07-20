
using System;

namespace Common.Serialization
{
    /// <summary>
    /// ���ͽ����ӿ�
    /// </summary>
    public abstract class JsonTypeResolver
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Type ResolveType(string id);
        /// <summary>
        /// �������͵�Id
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract string ResolveTypeId(Type type);
    }
}
