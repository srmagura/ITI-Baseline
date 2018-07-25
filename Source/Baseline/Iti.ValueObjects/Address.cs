using System.ComponentModel.DataAnnotations;
using Iti.Core.Validation;
using Iti.Core.ValueObjects;
using Iti.Utilities;

namespace Iti.ValueObjects
{
    public class Address : ValueObject<Address>
    {
        protected Address() { }

        public Address(string line1, string line2, string city, string state, string zip)
        {
            Require.NotEmpty(line1, "Invalid Address: Line1");
            Require.NotEmpty(city, "Invalid Address: City");
            Require.NotEmpty(state, "Invalid Address: State");
            Require.NotEmpty(zip, "Invalid Address: Zip");

            Line1 = line1?.Trim().MaxLength(FieldLengths.Address.Line1);
            Line2 = line2?.Trim().MaxLength(FieldLengths.Address.Line2);
            City = city?.Trim().MaxLength(FieldLengths.Address.City);
            State = state?.Trim().MaxLength(FieldLengths.Address.State);
            Zip = zip?.Trim().MaxLength(FieldLengths.Address.Zip);
        }

        [MaxLength(FieldLengths.Address.Line1)]
        public string Line1 { get; protected set; }

        [MaxLength(FieldLengths.Address.Line2)]
        public string Line2 { get; protected set; }

        [MaxLength(FieldLengths.Address.City)]
        public string City { get; protected set; }

        [MaxLength(FieldLengths.Address.State)]
        public string State { get; protected set; }

        [MaxLength(FieldLengths.Address.Zip)]
        public string Zip { get; protected set; }

        public override bool HasValue()
        {
            return HasAnyValue(Line1, Line2, City, State, Zip);
        }

        public override string ToString()
        {
            return $"{Line1}, {(Line2.HasValue() ? $"{Line2}," : "")} {City}, {State} {Zip}";
        }
    }
}