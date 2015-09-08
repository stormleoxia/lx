using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Lx.Tools.Common
{
    public static class HashSetEx
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
        {
            var hash = new HashSet<T>();
            foreach (var item in items)
            {
                hash.Add(item);
            }
            return hash;
        }

        public static Dictionary<TKey, TValue> ToDictionaryIgnoreDuplicates<TKey, TValue, TSource>(
            this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in enumerable)
            {
                var key = keySelector(item);
                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = valueSelector(item);
                }
            }
            return dictionary;
        }
    }
}