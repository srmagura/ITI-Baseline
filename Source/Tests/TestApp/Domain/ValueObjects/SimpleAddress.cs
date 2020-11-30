using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ITI.DDD.Core.Util;
using ITI.DDD.Domain.ValueObjects;
using TestApp.DataContext;

namespace TestApp.Domain.ValueObjects
{
    public class SimpleAddress : ValueObject
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
        public string? Line1 { get; protected set; }

        [MaxLength(TestAppFieldLengths.SimpleAddress.Line2)]
        public string? Line2 { get; protected set; }

        [MaxLength(TestAppFieldLengths.SimpleAddress.City)]
        public string? City { get; protected set; }

        [MaxLength(TestAppFieldLengths.SimpleAddress.State)]
        public string? State { get; protected set; }

        [MaxLength(TestAppFieldLengths.SimpleAddress.Zip)]
        public string? Zip { get; protected set; }

        public override string ToString()
        {
            var s = "";

            AddPart(ref s, Line1);
            AddPart(ref s, Line2);
            AddPart(ref s, City);
            AddPart(ref s, $"{State} {Zip}");

            return s.Replace("  ", " ").Trim();
        }

        private void AddPart(ref string s, string? value)
        {
            if (!value.HasValue())
                return;

            if (s.HasValue())
                s += ", ";
            s += value;
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Line1;
            yield return Line2;
            yield return City;
            yield return State;
            yield return Zip;
        }
    }
}