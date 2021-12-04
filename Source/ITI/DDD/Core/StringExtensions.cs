using System.Diagnostics.CodeAnalysis;

namespace ITI.DDD.Core
{
    public static class StringExtensions
    {
        public static string MaxLength(this string s, int maxLength)
        {
            return s.Length <= maxLength
                ? s
                : s[..maxLength];
        }

        public static bool HasValue([NotNullWhen(true)] this string? s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }
    }
}
