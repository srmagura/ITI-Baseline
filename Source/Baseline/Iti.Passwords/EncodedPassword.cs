using System.ComponentModel.DataAnnotations;
using Iti.Baseline.Core.Validation;
using Iti.Baseline.Core.ValueObjects;
using Iti.Baseline.Utilities;

namespace Iti.Baseline.Passwords
{
    public class EncodedPassword : ValueObject<EncodedPassword>, IEncodedPassword
    {
        protected EncodedPassword() { }

        internal EncodedPassword(string encodedValue)
        {
            Require.NotEmpty(encodedValue, "Invalid password (empty)");

            Value = encodedValue;
        }

        [MaxLength(128)]
        public string Value { get; protected set; }

        public override bool HasValue()
        {
            return Value.HasValue();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}