using Iti.Core.Exceptions;

namespace Iti.Core.Validation
{
    public static class Require
    {
        public static void NotNull(object obj, string message)
        {
            if (obj == null)
                throw new DomainException(message);
        }

        public static void NotEmpty(string s, string message)
        {
            s = s?.Trim();
            if (string.IsNullOrEmpty(s))
                throw new DomainException(message);
        }

        public static void IsTrue(bool b, string message)
        {
            if (!b)
                throw new DomainException(message);
        }

        public static void HasValue(string value, string name, int minLength, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException($"{name} is required");
            if (value.Length < minLength || value.Length > maxLength)
                throw new DomainException($"{name} must be between {minLength} and {maxLength} characters");
        }
    }
}
