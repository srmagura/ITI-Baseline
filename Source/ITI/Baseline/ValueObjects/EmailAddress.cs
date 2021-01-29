using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITI.Baseline.Util;
using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain.ValueObjects;

namespace ITI.Baseline.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        protected EmailAddress() { }

        public EmailAddress(string value)
        {
            Value = value.Trim();

            Require.HasValue(Value, "Email Address", 1, 256);
            Require.IsTrue(Value.IsValidEmail(), $"Invalid Email Address: {Value}");
        }

        [MaxLength(FieldLengths.EmailAddress.Value)]
        public string? Value { get; protected set; }

        public override string ToString()
        {
            return Value ?? "";
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }
}