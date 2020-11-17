using ITI.DDD.Core.Util;
using ITI.DDD.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITI.Baseline.ValueObjects
{
    public class SimplePersonName : ValueObject
    {
        [Obsolete("Persistence user only")]
        protected SimplePersonName() { }

        public SimplePersonName(string first, string? middle, string last, string? prefix = null)
        {
            Prefix = prefix?.Trim().MaxLength(FieldLengths.SimplePersonName.Prefix);
            First = first.Trim().MaxLength(FieldLengths.SimplePersonName.First);
            Middle = middle?.Trim().MaxLength(FieldLengths.SimplePersonName.Middle);
            Last = last.Trim().MaxLength(FieldLengths.SimplePersonName.Last);
        }

        //

        [MaxLength(FieldLengths.SimplePersonName.Prefix)]
        public string? Prefix { get; protected set; }

        [MaxLength(FieldLengths.SimplePersonName.First)]
        public string? First { get; protected set; }

        [MaxLength(FieldLengths.SimplePersonName.Middle)]
        public string? Middle { get; protected set; }

        [MaxLength(FieldLengths.SimplePersonName.Last)]
        public string? Last { get; protected set; }

        public override string ToString()
        {
            var s = $"{Prefix ?? ""} {Last}, {First} {Middle}";
            return s.Trim();
        }

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Prefix;
            yield return First;
            yield return Middle;
            yield return Last;
        }
    }
}