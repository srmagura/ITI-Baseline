using ITI.Baseline.ValueObjects;

namespace TestApp.Application.Dto;

public class PhoneNumberDto
{
    public PhoneNumberDto(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Value { get; set; }

    public PhoneNumber ToValueObject()
    {
        return new PhoneNumber(Value);
    }
}
