using System.ComponentModel.DataAnnotations;
using Iti.Core.Validation;
using Iti.Core.ValueObjects;
using Iti.Utilities;

namespace Iti.Passwords
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