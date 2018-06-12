using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Carpa.Web.Script;

namespace Common
{
    public static class HashObjectExt
    {
        internal class Key
        {
            public Key(string name)
            {
                Name = name;
                Children = new List<Key>();
            }
            public List<Key> Children { get; private set; }

            public string Name { get; private set; }
        }

        public class KeyValue : HashObject
        {

        }

        public static T GetDataEx<T>(this List<KeyValue> list, string key)
        {
            foreach (KeyValue keyValue in list)
            {
                if (keyValue.ContainsKey(key))
                {
                    object obj = keyValue[key];
                    return (T)obj;
                }
            }
            return default(T);
        }

        public static List<KeyValue> GetHashValue(this HashObject hash, string[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return null;
            }

            Key keytree = GetKeyTree(keys);

            return GetChildrenKeyValue(keytree, hash);
        }

        private static List<KeyValue> GetItemKeyValue(HashObject hash, Key key)
        {
            object data;
            if (!hash.TryGetValue(key.Name, out data))
            {
                return null;
            }

            if (key.Children.Count == 0)
            {
                KeyValue kv = new KeyValue();
                kv.Add(key.Name, data);
                return new List<KeyValue>() { kv };
            }

            List<KeyValue> list = GetListHashValue(key, data);

            return list ?? GetChildrenKeyValue(key, data);
        }

        private static List<KeyValue> GetListHashValue(Key key, object data)
        {
            if (!(data is object[]))
            {
                return null;
            }
            List<object> rt = new List<object>();
            object[] array = (object[])data;
            for (int i = 0; i < array.Length; i++)
            {
                object d = array[i];
                List<KeyValue> temp = GetChildrenKeyValue(key, d);
                if (temp == null || temp.Count == 0)
                {
                    continue;
                }
                rt.Add(RebuildRowChildData(temp));
            }

            KeyValue t = new KeyValue();
            t.Add(key.Name, rt);
            return new List<KeyValue>() { t };
        }

        private static KeyValue RebuildRowChildData(List<KeyValue> temp)
        {
            KeyValue row = new KeyValue();

            foreach (KeyValue tk in temp)
            {
                foreach (string k in tk.Keys)
                {
                    row[k] = tk[k];//重复值就不考虑了，属于设置不正确
                }
            }
            return row;
        }

        private static List<KeyValue> GetChildrenKeyValue(Key key, object data)
        {
            if (!(data is HashObject))
            {
                return null;
            }
            List<KeyValue> list = new List<KeyValue>();
            foreach (Key ckey in key.Children)
            {
                List<KeyValue> temp = GetItemKeyValue((HashObject)data, ckey);

                if (temp == null || temp.Count == 0)
                {
                    continue;
                }
                list.AddRange(temp.ToArray());
            }

            return list;
        }

        /// <summary>
        /// 获取键值树
        /// </summary>
        private static Key GetKeyTree(string[] keys)
        {
            Key root = new Key("");
            Dictionary<string, Key> dictionary = new Dictionary<string, Key>();
            dictionary.Add("", root);
            foreach (string key in keys)
            {
                string[] items = key.Split('/');
                string pre = "";
                for (int i = 0; i < items.Length; i++)
                {
                    string item = items[i];
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    string dkey = string.Format("{0}/{1}", pre, item);
                    Key ikey;
                    if (!dictionary.TryGetValue(dkey, out ikey))
                    {
                        ikey = new Key(item);
                        dictionary.Add(dkey, ikey);
                        dictionary[pre].Children.Add(ikey);
                    }
                    pre = string.Format("{0}/{1}", pre, item);
                }
            }

            return root;
        }
    }
}
