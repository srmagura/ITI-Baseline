using System;
using System.Linq;

namespace ITI.DDD.Core.Util
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string s, string value)
        {
            if (s == null)
                return value == null;

            return string.Compare(s, value, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public static string? MaxLength(this string? s, int maxLength)
        {
            if (s == null)
                return null;

            return s.Length < maxLength
                ? s
                : s.Substring(0, maxLength);
        }

        public static bool HasValue(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
    }
}
