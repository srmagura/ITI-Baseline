using System.Diagnostics.CodeAnalysis;

namespace ITI.Baseline.Util.Validation
{
    public static class Require
    {
        public static void NotNull([NotNull] object? obj, string message)
        {
            if (obj == null)
                throw new ValidationException(message);
        }

        public static void NotEmpty([NotNull] string? s, string message)
        {
            s = s?.Trim();
            if (string.IsNullOrEmpty(s))
                throw new ValidationException(message);
        }

        public static void IsTrue(bool b, string message)
        {
            if (!b)
                throw new ValidationException(message);
        }

        public static void HasValue([NotNull] string? value, string name, int minLength, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException($"{name} is required");
            if (value.Length < minLength || value.Length > maxLength)
                throw new ValidationException($"{name} must be between {minLength} and {maxLength} characters");
        }
    }
}
