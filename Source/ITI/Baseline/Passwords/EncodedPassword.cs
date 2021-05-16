using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITI.Baseline.Passwords
{
    public record EncodedPassword : ValueObject, IEncodedPassword
    {
        [Obsolete("Serialization Only", true)]
        protected EncodedPassword() { }

        internal EncodedPassword(string encodedValue)
        {
            Require.NotEmpty(encodedValue, "Invalid password (empty)");

            Value = encodedValue;
        }

        //

        [MaxLength(128)]
        public string? Value { get; protected set; }

        public override string ToString()
        {
            return Value;
        }
    }
}