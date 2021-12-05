using System.ComponentModel.DataAnnotations;
using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain;

namespace ITI.Baseline.ValueObjects
{
    public record PhoneNumber : DbValueObject
    {
        private static string DigitsOnly(string s)
        {
            return new string(s.Where(char.IsDigit).ToArray());
        }

        public static bool IsValidPhone(string s)
        {
            return DigitsOnly(s).Length >= 10;
        }

        public PhoneNumber(string value)
        {
            Require.IsTrue(IsValidPhone(value), $"Invalid phone number: {value}.");
            Value = DigitsOnly(value);
        }

        [MaxLength(FieldLengths.PhoneNumber.Value)]
        public string Value { get; protected init; }
    }
}