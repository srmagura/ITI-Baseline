using ITI.DDD.Core;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application.Dto;

public class PersonNameDto
{
    public PersonNameDto(string first, string? middle, string last)
    {
        First = first ?? throw new ArgumentNullException(nameof(first));
        Middle = middle;
        Last = last ?? throw new ArgumentNullException(nameof(last));
    }

    public string First { get; set; }
    public string? Middle { get; set; }
    public string Last { get; set; }

    public PersonName ToValueObject()
    {
        return new PersonName(
            First ?? throw new ValidationException("First"),
            Middle,
            Last ?? throw new ValidationException("Last")
        );
    }
}
