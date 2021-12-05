using ITI.DDD.Core;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application.Dto;

public class AddressDto
{
    public AddressDto(string line1, string? line2, string city, string state, string zip)
    {
        Line1 = line1 ?? throw new ArgumentNullException(nameof(line1));
        Line2 = line2;
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        Zip = zip ?? throw new ArgumentNullException(nameof(zip));
    }

    public string Line1 { get; set; }
    public string? Line2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }

    public Address ToValueObject()
    {
        return new Address(
            Line1 ?? throw new ValidationException("Line 1"),
            Line2,
            City ?? throw new ValidationException("Line 2"),
            State ?? throw new ValidationException("City"),
            Zip ?? throw new ValidationException("Zip")
        );
    }
}
