using System;
using System.Collections.Generic;
using System.Linq;

namespace GameNote.Core
{
    public static class Extensions
    {
        public static bool HasValue(this string input)
            => !string.IsNullOrEmpty(input);

        public static bool Empty<T>(this IEnumerable<T> items)
            => items.Any() == false;

        public static bool Remove<T>(this List<T> list, Func<T, bool> selector)
        {
            var item = list.SingleOrDefault(selector);

            if (item == null)
                return false;

            return list.Remove(item);
        }
    }
}