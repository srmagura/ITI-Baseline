using System.ComponentModel.DataAnnotations;
using Iti.Baseline.Core.Validation;
using Iti.Baseline.Core.ValueObjects;
using Iti.Baseline.Utilities;
using Newtonsoft.Json;

namespace Iti.Baseline.ValueObjects
{
    public class PhoneNumber : ValueObject<PhoneNumber>
    {
        protected PhoneNumber() { }

        [JsonConstructor]
        public PhoneNumber(string value)
        {
            value = value?.DigitsOnly();

            Require.IsTrue(value.IsValidPhone(), "Invalid phone number");
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