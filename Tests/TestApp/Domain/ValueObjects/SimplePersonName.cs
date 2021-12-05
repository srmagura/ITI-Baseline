using ITI.DDD.Domain;
using System.ComponentModel.DataAnnotations;
using TestApp.DataContext;

namespace TestApp.Domain.ValueObjects
{
    public record SimplePersonName : DbValueObject
    {
        public SimplePersonName(string first, string? middle, string last)
        {
            First = first;
            Middle = middle;
            Last = last;
        }

        [MaxLength(TestAppFieldLengths.SimplePersonName.First)]
        public string First { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimplePersonName.Middle)]
        public string? Middle { get; protected init; }

        [MaxLength(TestAppFieldLengths.SimplePersonName.Last)]
        public string Last { get; protected init; }

        public override string ToString()
        {
            return $"{Last}, {First} {Middle}".Trim();
        }
    }
}