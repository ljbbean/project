using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Script
{
    public static class ObjectExt
    {
        public static T GetDataEx<T>(this List<Common.Script.HashMap.KeyValue> list, string key)
        {
            foreach (Common.Script.HashMap.KeyValue keyValue in list)
            {
                if (keyValue.ContainsKey(key))
                {
                    object obj = keyValue[key];
                    return (T)obj;
                }
            }
            return default(T);
        }
    }
}
