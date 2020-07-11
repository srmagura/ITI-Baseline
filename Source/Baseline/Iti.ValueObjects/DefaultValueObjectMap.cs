using AutoMapper;
using Iti.Baseline.Passwords;

namespace Iti.Baseline.ValueObjects
{
    public static class DefaultValueObjectMap
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<EncodedPassword, EncodedPassword>();
            cfg.CreateMap<EmailAddress, EmailAddress>();

            cfg.CreateMap<SimpleAddress, SimpleAddress>();
            cfg.CreateMap<SimplePersonName, SimplePersonName>();

            cfg.CreateMap<PhoneNumber, PhoneNumber>();
        }
    }
}