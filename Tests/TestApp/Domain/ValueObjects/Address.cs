using System.ComponentModel.DataAnnotations;
using ITI.DDD.Core;
using ITI.DDD.Domain;
using TestApp.DataContext;

namespace TestApp.Domain.ValueObjects;

public record Address : DbValueObject
{
    public Address(string line1, string? line2, string city, string state, string zip)
    {
        Line1 = line1;
        Line2 = line2;
        City = city;
        State = state;
        Zip = zip;
    }

    [MaxLength(TestAppFieldLengths.Address.Line1)]
    public string Line1 { get; protected init; }

    [MaxLength(TestAppFieldLengths.Address.Line2)]
    public string? Line2 { get; protected init; }

    [MaxLength(TestAppFieldLengths.Address.City)]
    public string City { get; protected init; }

    [MaxLength(TestAppFieldLengths.Address.State)]
    public string State { get; protected init; }

    [MaxLength(TestAppFieldLengths.Address.Zip)]
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
