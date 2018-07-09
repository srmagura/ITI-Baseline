using System.Collections.Generic;

namespace Iti.Utilities
{
    public static class ListExtensions
    {
        public static bool HasItems<T>(this List<T> list)
        {
            return list != null && list.Count > 0;
        }

        public static string ToDelimited<T>(this List<T> list, string delim, bool wrap = true)
        {
            var s = string.Join(delim, list);
            if (!s.HasValue())
                return "";
            return wrap ? $"{delim}{s}{delim}" : s;
        }
    }
}