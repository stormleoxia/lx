using System.Collections.Generic;

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
    }
}