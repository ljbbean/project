
using System;

namespace Common.Serialization
{
    /// <summary>
    /// 类型解析接口
    /// </summary>
    public abstract class JsonTypeResolver
    {
        /// <summary>
        /// 解析类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Type ResolveType(string id);
        /// <summary>
        /// 解析类型的Id
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract string ResolveTypeId(Type type);
    }
}
