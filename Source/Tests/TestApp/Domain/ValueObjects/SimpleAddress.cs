using System.ComponentModel.DataAnnotations;
using ITI.DDD.Domain.ValueObjects;

namespace TestApp.Domain.ValueObjects
{
    public class SimpleAddress : ValueObject<SimpleAddress>
    {
        protected SimpleAddress() { }

        [JsonConstructor]
        public SimpleAddress(string line1, string line2, string city, string state, string zip)
        {
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
            var s = "";

            AddPart(ref s, Line1);
            AddPart(ref s, Line2);
            AddPart(ref s, City);
            AddPart(ref s, $"{State} {Zip}");

            return s.Replace("  ", " ").Trim();
        }

        private void AddPart(ref string s, string value)
        {
            if (!value.HasValue())
                return;

            if (s.HasValue())
                s += ", ";
            s += value;
        }
    }
}