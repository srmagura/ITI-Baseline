using TestApp.Domain.ValueObjects;

namespace TestApp.Application.Dto;

public class FacilityContactDto
{
    public PersonNameDto? Name { get; set; }
    public EmailAddressDto? Email { get; set; }

    public FacilityContact ToValueObject()
    {
        return new FacilityContact(
            Name?.ToValueObject(),
            Email?.ToValueObject()
        );
    }
}
