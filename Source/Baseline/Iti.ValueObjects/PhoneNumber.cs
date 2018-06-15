using System.ComponentModel.DataAnnotations;
using Iti.Core.Validation;
using Iti.Core.ValueObjects;
using Iti.Utilities;

namespace Iti.ValueObjects
{
    public class PhoneNumber : ValueObject<PhoneNumber>
    {
        protected PhoneNumber() { }

        public PhoneNumber(string value)
        {
            value = value?.DigitsOnly();

            Require.NotNull(value.IsValidPhone(), "Invalid phone number");
            Value = value.MaxLength(FieldLengths.PhoneNumber.Value);
        }

        [MaxLength(FieldLengths.PhoneNumber.Value)]
        public string Value { get; protected set; }

        public override bool HasValue()
        {
            return Value.HasValue();
        }
    }
}