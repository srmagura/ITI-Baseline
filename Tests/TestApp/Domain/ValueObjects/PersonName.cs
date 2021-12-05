using System.ComponentModel.DataAnnotations;
using ITI.DDD.Domain;
using TestApp.DataContext;

namespace TestApp.Domain.ValueObjects;

public record PersonName : DbValueObject
{
    public PersonName(string first, string? middle, string last)
    {
        First = first;
        Middle = middle;
        Last = last;
    }

    [MaxLength(TestAppFieldLengths.PersonName.First)]
    public string First { get; protected init; }

    [MaxLength(TestAppFieldLengths.PersonName.Middle)]
    public string? Middle { get; protected init; }

    [MaxLength(TestAppFieldLengths.PersonName.Last)]
    public string Last { get; protected init; }

    public override string ToString()
    {
        return $"{Last}, {First} {Middle}".Trim();
    }
}
