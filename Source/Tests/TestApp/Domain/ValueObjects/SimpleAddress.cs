using System;
using System.ComponentModel.DataAnnotations;
using ITI.DDD.Core.Util;
using ITI.DDD.Domain.ValueObjects;
using TestApp.DataContext;

namespace TestApp.Domain.ValueObjects
{
    public record SimpleAddress : DbValueObject
    {
        [Obsolete("Persistence user only")]
        protected SimpleAddress() { }

        public SimpleAddress(string line1, string? line2, string city, string state, string zip)
        {
            Line1 = line1.Trim().MaxLength(TestAppFieldLengths.SimpleAddress.Line1);
            Line2 = line2?.Trim().MaxLength(TestAppFieldLengths.SimpleAddress.Line2);
            City = city.Trim().MaxLength(TestAppFieldLengths.SimpleAddress.City);
            State = state.Trim().MaxLength(TestAppFieldLengths.SimpleAddress.State);
            Zip = zip.Trim().MaxLength(TestAppFieldLengths.SimpleAddress.Zip);
        }

        [MaxLength(TestAppFieldLengths.SimpleAddress.Line1)]
        public string Line1 { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimpleAddress.Line2)]
        public string? Line2 { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimpleAddress.City)]
        public string City { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimpleAddress.State)]
        public string State { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimpleAddress.Zip)]
        public string Zip { get; protected init; }

        public override string ToString()
        {
            var s = "";

            AddPart(ref s, Line1);
            AddPart(ref s, Line2);
            AddPart(ref s, City);
            AddPart(ref s, $"{State} {Zip}");

            return s.Replace("  ", " ").Trim();
        }

        private static void AddPart(ref string s, string? value)
        {
            if (!value.HasValue())
                return;

            if (s.HasValue())
                s += ", ";
            s += value;
        }
    }
}