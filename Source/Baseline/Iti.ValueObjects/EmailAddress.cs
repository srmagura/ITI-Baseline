using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting.Messaging;
using Iti.Core.Validation;
using Iti.Core.ValueObjects;
using Iti.Utilities;

namespace Iti.ValueObjects
{
    public class EmailAddress : ValueObject<EmailAddress>
    {
        protected EmailAddress() { }
        public EmailAddress(string value)
        {
            Value = value?.Trim();

            Require.HasValue(Value, "Email Address", 1, 256);
            Require.IsTrue(Value.IsValidEmail(), $"Invalid Email Address: {Value}");
        }

        [MaxLength(FieldLengths.EmailAddress.Value)]
        public string Value { get; protected set; }

        public override string ToString()
        {
            return Value;
        }

        public override bool HasValue()
        {
            return Value.HasValue();
        }
    }
}