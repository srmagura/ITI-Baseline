using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Baseline.Passwords
{
    public record EncodedPassword : DbValueObject
    {
        public EncodedPassword(string value)
        {
            Require.NotEmpty(value, "Invalid encoded password (empty).");

            Value = value;
        }

        [MaxLength(128)]
        public string Value { get; protected init; }

        public override string ToString()
        {
            return Value;
        }
    }
}