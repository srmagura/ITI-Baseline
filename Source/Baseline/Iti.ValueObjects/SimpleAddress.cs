using System.ComponentModel.DataAnnotations;
using Iti.Core.Validation;
using Iti.Core.ValueObjects;
using Iti.Utilities;
using Newtonsoft.Json;

namespace Iti.ValueObjects
{
    public class SimpleAddress : ValueObject<SimpleAddress>
    {
        protected SimpleAddress() { }

        // TODO:JT:XXX: move out of Baseline

        [JsonConstructor]
        public SimpleAddress(string line1, string line2, string city, string state, string zip)
        {
            Require.NotEmpty(line1, "Invalid Address: Line1");
            Require.NotEmpty(city, "Invalid Address: City");
            Require.NotEmpty(state, "Invalid Address: State");
            Require.NotEmpty(zip, "Invalid Address: Zip");

            Line1 = line1?.Trim().MaxLength(FieldLengths.SimpleAddress.Line1);
            Line2 = line2?.Trim().MaxLength(FieldLengths.SimpleAddress.Line2);
            City = city?.Trim().MaxLength(FieldLengths.SimpleAddress.City);
            State = state?.Trim().MaxLength(FieldLengths.SimpleAddress.State);
            Zip = zip?.Trim().MaxLength(FieldLengths.SimpleAddress.Zip);
        }

        [MaxLength(FieldLengths.SimpleAddress.Line1)]
        public string Line1 { get; protected set; }

        [MaxLength(FieldLengths.SimpleAddress.Line2)]
        public string Line2 { get; protected set; }

        [MaxLength(FieldLengths.SimpleAddress.City)]
        public string City { get; protected set; }

        [MaxLength(FieldLengths.SimpleAddress.State)]
        public string State { get; protected set; }

        [MaxLength(FieldLengths.SimpleAddress.Zip)]
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