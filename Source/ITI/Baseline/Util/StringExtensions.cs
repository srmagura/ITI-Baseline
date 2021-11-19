using ITI.DDD.Core.Util;
using System.Linq;

namespace ITI.Baseline.Util
{
    public static class StringExtensions
    {
        public static bool IsValidEmail(this string? email)
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

        public static bool IsValidPhone(this string? s)
        {
            return s?.Count(char.IsDigit) >= 10;
        }

        public static string DigitsOnly(this string s)
        {
            return new string(s.Where(char.IsDigit).ToArray());
        }
    }
}
