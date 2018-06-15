using System;
using System.Linq;
using System.Xml;

namespace Iti.Utilities
{
    public class DebugTimer : IDisposable
    {
        private readonly string _name;
        private readonly DateTime _start;
        private readonly Action<string, TimeSpan> _output;

        public DebugTimer(string name, Action<string, TimeSpan> output = null)
        {
            _name = name;
            _start = DateTime.Now;

            if (output == null)
                output = (tag, ts) => { Console.WriteLine($"{tag}: {ts}"); };
            _output = output;
        }

        public void Dispose()
        {
            var end = DateTime.Now;
            var diff = end - _start;

            _output?.Invoke(_name, diff);
        }
    }

    public static class StringExtensions
    {
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
