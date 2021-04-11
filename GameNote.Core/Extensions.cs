using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
    }
}