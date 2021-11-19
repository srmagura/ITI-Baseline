using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Baseline.Passwords
{
    public record EncodedPassword : DbValueObject
    {
        public EncodedPassword(string encodedValue)
        {
            Require.NotEmpty(encodedValue, "Invalid encoded password (empty).");

            Value = encodedValue;
        }

        [MaxLength(128)]
        public string Value { get; protected init; }

        public override string ToString()
        {
            return Value;
        }
    }
}