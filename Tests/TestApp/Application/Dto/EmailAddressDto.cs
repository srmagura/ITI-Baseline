using ITI.Baseline.ValueObjects;

namespace TestApp.Application.Dto;

public class EmailAddressDto
{
    public EmailAddressDto(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Value { get; set; }

    public EmailAddress ToValueObject()
    {
        return new EmailAddress(Value);
    }
}
