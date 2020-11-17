using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITI.Baseline.Util;
using ITI.Baseline.Util.Validation;
using ITI.DDD.Core.Util;
using ITI.DDD.Domain.ValueObjects;

namespace ITI.Baseline.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        [Obsolete("Persistence user only")]
        protected PhoneNumber() { }

        public PhoneNumber(string value)
        {
            value = value.DigitsOnly();

            Require.IsTrue(value.IsValidPhone(), "Invalid phone number");
            Value = value.MaxLength(FieldLengths.PhoneNumber.Value);
        }

        [MaxLength(FieldLengths.PhoneNumber.Value)]
        public string? Value { get; protected set; }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }
}