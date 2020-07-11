using System;
using System.Linq;

namespace Iti.Utilities
{
    public static class StringExtensions
    {
        public static bool EqualsIgnoreCase(this string s, string value)
        {
            if (s == null)
                return value == null;

            return string.Compare(s, value, StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public static string MaxLength(this string s, int maxLength)
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

        public static bool IsValidEmail(this string email)
        {
            try
            {
                if (!email.HasValue())
                    return false;

                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhone(this string s)
        {
            return s?.Count(char.IsDigit) >= 10;
        }

        public static string DigitsOnly(this string s)
        {
            return s == null ? null : new string(s.Where(char.IsDigit).ToArray());
        }
    }
}
