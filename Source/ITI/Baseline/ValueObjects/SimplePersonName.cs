using ITI.DDD.Core.Util;
using ITI.DDD.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ITI.Baseline.ValueObjects
{
    public record SimplePersonName : ValueObject
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
        public string? Prefix { get; protected init; }

        [MaxLength(FieldLengths.SimplePersonName.First)]
        public string? First { get; protected init; }

        [MaxLength(FieldLengths.SimplePersonName.Middle)]
        public string? Middle { get; protected init; }

        [MaxLength(FieldLengths.SimplePersonName.Last)]
        public string? Last { get; protected init; }

        public override string ToString()
        {
            var s = $"{Prefix ?? ""} {Last}, {First} {Middle}";
            return s.Trim();
        }
    }
}