using ITI.Baseline.Util.Validation;
using ITI.DDD.Domain.ValueObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iti.Baseline.Passwords
{
    public class EncodedPassword : ValueObject, IEncodedPassword
    {
        protected EncodedPassword() { }

        internal EncodedPassword(string encodedValue)
        {
            Require.NotEmpty(encodedValue, "Invalid password (empty)");

            Value = encodedValue;
        }

        [MaxLength(128)]
        public string Value { get; protected set; }

        public override string ToString()
        {
            return Value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}