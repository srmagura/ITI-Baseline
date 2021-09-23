using System.ComponentModel.DataAnnotations;
using ITI.Baseline.Util;
using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain.ValueObjects;

namespace ITI.Baseline.ValueObjects
{
    public record EmailAddress : DbValueObject
    {
        protected EmailAddress() { }

        public EmailAddress(string value)
        {
            Value = value.Trim();

            Require.HasValue(Value, "Email Address", 1, FieldLengths.EmailAddress.Value);
            Require.IsTrue(Value.IsValidEmail(), $"Invalid Email Address: {Value}");
        }

        [MaxLength(FieldLengths.EmailAddress.Value)]
        public string? Value { get; protected init; }

        public override string ToString()
        {
            return Value ?? "";
        }
    }
}