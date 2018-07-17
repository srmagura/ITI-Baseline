using AutoMapper;
using Iti.Core.DateTime;
using Iti.Passwords;

namespace Iti.ValueObjects
{
    public static class DefaultValueObjectMap
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<EncodedPassword, EncodedPassword>();
            cfg.CreateMap<Address, Address>();
            cfg.CreateMap<EmailAddress, EmailAddress>();
            cfg.CreateMap<PersonName, PersonName>();
            cfg.CreateMap<PhoneNumber, PhoneNumber>();
        }
    }
}