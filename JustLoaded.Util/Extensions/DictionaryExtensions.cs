namespace JustLoaded.Util.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// TODO documentation
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue? Put<TKey, TValue>(this Dictionary<TKey, TValue> dict, in TKey key, in TValue value) where TKey : notnull {
            if (dict.TryGetValue(key, out var existing))
            {
                dict[key] = value;
                return existing;
            }
            dict.Add(key, value);
            return default;
        }
        
        public static void AddNested<TKey, TValue, TCollection>(this Dictionary<TKey, TCollection> dict, in TKey key, in TValue value) where TKey : notnull where TCollection : ICollection<TValue>, new() {
            var list = dict.GetValueOrSetDefaultLazy(key, () => new TCollection());
            list.Add(value);
        }

        public static void ClearNested<TKey, TValue, TCollection>(this Dictionary<TKey, TCollection> dict) where TKey : notnull where TCollection : ICollection<TValue> {
            foreach(var col in dict.Values)
            {
                col.Clear();
            }
        }

        public static TValue GetValueOrDefaultLazy<TKey, TValue>(this Dictionary<TKey, TValue> dict, in TKey key, Func<TValue> defaultValueLazy) where TKey : notnull {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }
            var v = defaultValueLazy();
            return v;
        }

        public static TValue GetValueOrSetDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, in TKey key, TValue defaultValue) where TKey : notnull {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }
            dict.Add(key, defaultValue);
            return defaultValue;
        }

        public static TValue GetValueOrSetDefaultLazy<TKey, TValue>(this Dictionary<TKey, TValue> dict, in TKey key, Func<TValue> defaultValueLazy) where TKey : notnull {
            if (dict.TryGetValue(key, out var value))
            {
                return value;
            }
            var v = defaultValueLazy();
            dict.Add(key, v);
            return v;
        }

        public static int PreIncrement<TKey>(this Dictionary<TKey, int> dict, in TKey key, int startValue = 0) where TKey : notnull {
            dict.TryAdd(key, startValue);
            return ++dict[key];
        }
        
        public static int PostIncrement<TKey>(this Dictionary<TKey, int> dict, in TKey key, int startValue = 0) where TKey : notnull {
            dict.TryAdd(key, startValue);
            return dict[key]++;
        }
    }
}
