using ITI.DDD.Core.Util;
using ITI.DDD.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TestApp.DataContext;

namespace ITI.Baseline.ValueObjects
{
    public record SimplePersonName : DbValueObject
    {
        [Obsolete("Persistence user only")]
        protected SimplePersonName() { }

        public SimplePersonName(string first, string? middle, string last, string? prefix = null)
        {
            Prefix = prefix?.Trim().MaxLength(TestAppFieldLengths.SimplePersonName.Prefix);
            First = first.Trim().MaxLength(TestAppFieldLengths.SimplePersonName.First);
            Middle = middle?.Trim().MaxLength(TestAppFieldLengths.SimplePersonName.Middle);
            Last = last.Trim().MaxLength(TestAppFieldLengths.SimplePersonName.Last);
        }

        //

        [MaxLength(TestAppFieldLengths.SimplePersonName.Prefix)]
        public string? Prefix { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimplePersonName.First)]
        public string? First { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimplePersonName.Middle)]
        public string? Middle { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimplePersonName.Last)]
        public string? Last { get; protected init; }

        public override string ToString()
        {
            var s = $"{Prefix ?? ""} {Last}, {First} {Middle}";
            return s.Trim();
        }
    }
}