
using Common.Entity;
using Common.Part;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Script
{
    /// <summary>
    /// 哈希对象
    /// </summary>
    public interface IHashMap : IDictionary<string, object>
    {
        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        IHashMap Clone();

        /// <summary>
        /// 添加多个项
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        void Add(string[] keys, params object[] values);
        /// <summary>
        /// 拷贝所有数据到另一个实例
        /// </summary>
        /// <param name="dict"></param>
        void CopyTo(IDictionary<string, object> dict);

        /// <summary>
        /// 取指定的值，默认值为null或0或false或空字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 取指定的值，找不到则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        T Get<T>(string key, T defaultValue);

        /// <summary>
        /// key存在则保持已有值不变返回false；不存在则设置并返回 true
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool CheckSetValue(string key, object value);

        /// <summary>
        /// 将HashObject转为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ToEntity<T>() where T : class,new();
    }

    /// <summary>
    /// 哈希对象列表
    /// </summary>
    public interface IHashMapList : IList<IHashMap>
    {
        /// <summary>
        /// 添加一个对象
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        void Add(string[] keys, params object[] values);

        /// <summary>
        /// 取一个范围，参数不对会抛出异常
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        IHashMapList GetRange(int index, int count);

        /// <summary>
        /// 按某个字段排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ascending">是否升序</param>
        void Sort(string key, bool ascending);

        /// <summary>
        /// 建索引快速查找
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IHashMap Find(string key, object value);

        /// <summary>
        /// 将HashObjectList转为实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IList<T> ToEntities<T>() where T : class,new();
    }

    /// <summary>
    /// 哈希对象
    /// </summary>
    [Serializable]
    public class HashMap : Dictionary<string, object>, IHashMap
    {
        /// <summary>
        /// 默认
        /// </summary>
        public HashMap()
            : base()
        {
        }

        /// <summary>
        /// 从已有数据复制
        /// </summary>
        /// <param name="dictionary"></param>
        public HashMap(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// 序列化构建函数，必须有这个函数才能反序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected HashMap(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// 传入许多键和值
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        public HashMap(string[] keys, params object[] values)
        {
            InternalAdd(this, keys, values);
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public IHashMap Clone()
        {
            HashMap clone = new HashMap();
            foreach (KeyValuePair<string, object> pair in this)
            {
                clone[pair.Key] = pair.Value; // 不能用Add,避免其它线程修改this，偶尔出现bug“An item with the same key has already been added”
            }
            return clone;
        }

        /// <summary>
        /// 添加条目
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        public void Add(string[] keys, params object[] values)
        {
            InternalAdd(this, keys, values);
        }

        /// <summary>
        /// 存取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new object this[string key]
        {
            get
            {
                try
                {
                    return base[key];
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException(string.Format("关键字“{0}”不在HashObject中。", key));
                }
            }
            set
            {
                base[key] = value;
            }
        }

        /// <summary>
        /// 检查是否设定某个key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CheckSetValue(string key, object value)
        {
            if (this.ContainsKey(key))
            {
                return false;
            }
            this[key] = value;
            return true;
        }

        /// <summary>
        /// 重写只是为了避免警告
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is HashMap))
                return false;

            HashMap that = (HashMap)obj;
            if (this.Count != that.Count)
                return false;
            foreach (string name in this.Keys)
            {
                if (!that.ContainsKey(name))
                    return false;
                object aValue = this[name];
                object bValue = that[name];
                if ((aValue != null) || (bValue != null))
                {
                    if (!object.Equals(this[name], that[name]))
                        return false;
                }
            }

            return true;
        }

        private static void InternalAdd(HashMap obj, string[] keys, object[] values)
        {
            if (keys.Length != values.Length)
                throw new InvalidOperationException("Keys和Values的长度不一致！");
            if (keys.Length == 0)
                throw new InvalidOperationException("添加数据必须有一项！");

            for (int i = 0; i < keys.Length; i++)
            {
                obj.Add(keys[i], values[i]);
            }
        }

        /// <summary>
        /// 拷贝所有项到
        /// </summary>
        /// <param name="dict"></param>
        public void CopyTo(IDictionary<string, object> dict)
        {
            foreach (KeyValuePair<string, object> pair in this)
            {
                dict[pair.Key] = pair.Value;
            }
        }
        /// <summary>
        /// 取指定的值，默认值为null或0或false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return Get<T>(key, default(T));
        }

        /// <summary>
        /// 取指定的值，找不到则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Get<T>(string key, T defaultValue)
        {
            try
            {
                object value;
                if (this.TryGetValue(key, out value))
                {
                    Type type = typeof(T);
                    Type underlyingType = type;
                    if (value == DBNull.Value)
                    {
                        return defaultValue;
                    }
                    else if (value != null && ReflectionUtils.IsPrimitiveType(type, out underlyingType))
                    {
                        return (T)ReflectionUtils.ChangeType(value, underlyingType);
                    }
                    else if (value != null && underlyingType != type &&
                        underlyingType.IsEnum && !value.GetType().IsEnum) // 整数转成 Nullable<enum>
                    {
                        return (T)Enum.ToObject(underlyingType, value);
                    }
                    else if (value == null && underlyingType.IsSubclassOf(typeof(ValueType)))
                    {
                        return defaultValue; // null转成ulong
                    }
                    else // 只能强转，可能出错
                    {
                        return (T)value;
                    }
                }
            }
            catch (Exception)
            {
                // 吃掉异常，by zq 2014-12-19 解决空字符串或非数值类型转ulong、转int、转uint都会抛出异常
            }

            return defaultValue; // 返回默认值
        }

        /// <summary>
        /// 将HashObject转为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ToEntity<T>() where T : class, new()
        {
            return EntityExtension.ToEntity<T>(this);
        }

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

        public class KeyValue : HashMap
        {

        }

        public List<KeyValue> GetHashValue(string[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return null;
            }

            Key keytree = GetKeyTree(keys);

            return GetChildrenKeyValue(keytree, this);
        }

        public List<KeyValue> GetHashValue(string key)
        {
            string[] keys = new string[] { key };
            return GetHashValue(keys);
        }

        private List<KeyValue> GetItemKeyValue(HashMap hash, Key key)
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

        private List<KeyValue> GetListHashValue(Key key, object data)
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

        private KeyValue RebuildRowChildData(List<KeyValue> temp)
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

        private List<KeyValue> GetChildrenKeyValue(Key key, object data)
        {
            if (!(data is HashMap))
            {
                return null;
            }
            List<KeyValue> list = new List<KeyValue>();
            foreach (Key ckey in key.Children)
            {
                List<KeyValue> temp = GetItemKeyValue((HashMap)data, ckey);

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
        private Key GetKeyTree(string[] keys)
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

        public string GetKey(int index)
        {
            return this.GetKeys(index);
        }

        private string GetKeys(int index)
        {
            if (index >= base.Keys.Count)
            {
                throw new IndexOutOfRangeException("索引位置超过键值集合大小");
            }
            int num = 0;
            foreach (string current in base.Keys)
            {
                if (num == index)
                {
                    return current;
                }
                num++;
            }
            return "";
        }
        public string GetStringValue(string key)
        {
            return this.Get<string>(key);
        }

        public int GetIntValue(string key)
        {
            return this.Get<int>(key);
        }

        public long GetLongValue(string key)
        {
            return this.Get<long>(key);
        }

        public bool GetBooleanValue(string key)
        {
            return this.Get<bool>(key);
        }

        public double GetDoubleValue(string key)
        {
            return this.Get<double>(key);
        }

        public float GetFloatValue(string key)
        {
            return this.Get<float>(key);
        }

        public byte GetByteValue(string key)
        {
            return this.Get<byte>(key);
        }

        public DateTime GetDateTimeValue(string key)
        {
            return this.Get<DateTime>(key);
        }

        public Array GetArrayValue(string key)
        {
            return this.Get<Array>(key);
        }
    }

    /// <summary>
    /// 哈希对象列表
    /// </summary>
    [Serializable]
    public class HashObjectList : List<IHashMap>, IHashMapList
    {
        private bool hasLongValue;

        /// <summary>
        /// 默认
        /// </summary>
        public HashObjectList()
            : base()
        {
        }

        /// <summary>
        /// 传入出示大小
        /// </summary>
        /// <param name="capacity"></param>
        public HashObjectList(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// 添加许多项
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        public void Add(string[] keys, params object[] values)
        {
            HashMap obj = new HashMap(keys, values);
            Add(obj);
        }

        /// <summary>
        /// 重写只是为了避免警告
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is HashObjectList))
                return false;

            HashObjectList that = (HashObjectList)obj;
            if (this.Count != that.Count)
                return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (!(this[i].Equals(that[i])))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 取一个范围，参数不对会抛出异常
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public new IHashMapList GetRange(int index, int count)
        {
            List<IHashMap> list = base.GetRange(index, count);
            IHashMapList answer = new HashObjectList(count);
            foreach (IHashMap item in list)
            {
                answer.Add(item);
            }
            return answer;
        }

        /// <summary>
        /// 按某个字段排序
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ascending">是否升序</param>
        public void Sort(string key, bool ascending)
        {
            base.Sort(delegate(IHashMap obj1, IHashMap obj2)
            {
                IComparable cmp1 = obj1.Get<IComparable>(key);
                IComparable cmp2 = obj2.Get<IComparable>(key);

                if (cmp1 == null) return ascending ? -1 : 1;
                if (cmp2 == null) return ascending ? 1 : -1;

                return ascending ? cmp1.CompareTo(cmp2) : cmp2.CompareTo(cmp1);
            });
        }

        [NonSerialized]
        private IDictionary<string, Hashtable> indexMap;

        /// <summary>
        /// 添加项
        /// </summary>
        /// <param name="item"></param>
        public new void Add(IHashMap item)
        {
            base.Add(item);
            if (indexMap != null)
            {
                AddIndexItem(item);
            }
        }

        /// <summary>
        /// 插入项
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public new void Insert(int index, IHashMap item)
        {
            base.Insert(index, item);
            if (indexMap != null)
            {
                AddIndexItem(item);
            }
        }

        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public new bool Remove(IHashMap item)
        {
            bool answer = base.Remove(item);
            if (indexMap != null)
            {
                RemoveIndexItem(item);
            }
            return answer;
        }

        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="index"></param>
        public new void RemoveAt(int index)
        {
            if (indexMap != null)
            {
                RemoveIndexItem(this[index]);
            }
            base.RemoveAt(index);
        }

        /// <summary>
        /// 移除范围
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            indexMap = null; // 清除索引
        }

        /// <summary>
        /// 移除特定
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public new int RemoveAll(Predicate<IHashMap> match)
        {
            indexMap = null; // 清除索引
            return RemoveAll(match);
        }

        /// <summary>
        /// 移除范围
        /// </summary>
        /// <param name="collection"></param>
        public new void AddRange(IEnumerable<IHashMap> collection)
        {
            base.AddRange(collection);
            indexMap = null; // 清除索引
        }

        /// <summary>
        /// 插入范围
        /// </summary>
        /// <param name="index"></param>
        /// <param name="collection"></param>
        public new void InsertRange(int index, IEnumerable<IHashMap> collection)
        {
            base.InsertRange(index, collection);
            indexMap = null; // 清除索引
        }

        /// <summary>
        /// 清除
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            indexMap = null; // 清除索引
        }


        private void AddIndexItem(IHashMap item)
        {
            if (indexMap == null) return;
            foreach (KeyValuePair<string, Hashtable> pair in indexMap)
            {
                string key = pair.Key;
                Hashtable index = pair.Value;
                object value;
                if (item.TryGetValue(key, out value))
                {
                    index[value] = item;
                }
            }
        }

        private void RemoveIndexItem(IHashMap item)
        {
            if (indexMap == null) return;
            foreach (KeyValuePair<string, Hashtable> pair in indexMap)
            {
                string key = pair.Key;
                Hashtable index = pair.Value;
                object value;
                if (item.TryGetValue(key, out value))
                {
                    index.Remove(value);
                }
            }
        }

        /// <summary>
        /// 建索引快速查找
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public IHashMap Find(string key, object value)
        {
            Hashtable index = GetIndex(key);
            IHashMap answer = (IHashMap)index[value];
            if (answer == null && value != null && value.GetType() == typeof(ulong) && hasLongValue) // 对ulong主关键字查找做一次特殊处理，因为从数据库读出来的是long
            {
                long l = Convert.ToInt64(value);
                answer = (IHashMap)index[l];
            }
            return answer;
        }

        private Hashtable GetIndex(string key)
        {
            if (indexMap == null)
            {
                indexMap = new Dictionary<string, Hashtable>();
            }
            Hashtable index;
            if (!indexMap.TryGetValue(key, out index))
            {
                index = new Hashtable();
                indexMap[key] = index;
                BuildIndex(key, index);
            }
            return index;
        }

        private void BuildIndex(string key, Hashtable index)
        {
            hasLongValue = false;
            foreach (IHashMap item in this)
            {
                object value;
                if (item.TryGetValue(key, out value))
                {
                    if (!hasLongValue && value != null && value.GetType() == typeof(long))
                    {
                        hasLongValue = true;
                    }
                    index[value] = item;
                }
            }
        }

        /// <summary>
        /// 将HashObjectList转为实体列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> ToEntities<T>() where T : class, new()
        {
            IList<T> entities = new List<T>();
            foreach (IHashMap ho in this)
            {
                entities.Add(ho.ToEntity<T>());
            }
            return entities;
        }
    }
}
