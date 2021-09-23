using ITI.Baseline.Util.Validation;
using ITI.Baseline.ValueObjects;
using TestApp.Domain.ValueObjects;

namespace TestApp.Application.Dto
{
    public static class DtoManualMappingExtensions
    {
        public static SimplePersonName ToValueObject(this PersonNameDto name)
        {         
            return new SimplePersonName(
                name.First ?? throw new ValidationException("First"), 
                name.Middle, 
                name.Last ?? throw new ValidationException("Last"), 
                name.Prefix
            );
        }

        public static EmailAddress ToValueObject(this EmailAddressDto emailAddress)
        {
            return new EmailAddress(emailAddress.Value ?? throw new ValidationException("Email"));
        }

        public static PhoneNumber ToValueObject(this PhoneNumberDto phoneNumber)
        {
            return new PhoneNumber(phoneNumber.Value ?? throw new ValidationException("Phone"));
        }

        public static SimpleAddress ToValueObject(this AddressDto address)
        {
            return new SimpleAddress(
                address.Line1 ?? throw new ValidationException("Line 1"),
                address.Line2,
                address.City ?? throw new ValidationException("Line 2"),
                address.State ?? throw new ValidationException("City"),
                address.Zip ?? throw new ValidationException("Zip")
            );
        }

        public static FacilityContact ToValueObject(this FacilityContactDto contact)
        {
            return new FacilityContact(
                contact.Name?.ToValueObject(),
                contact.Email?.ToValueObject()
            );
        }
    }
}
