using AutoMapper;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using ITI.DDD.Infrastructure.DataMapping;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application.Dto;
using TestApp.DataContext.DataModel;
using TestApp.Domain;
using TestApp.Domain.ValueObjects;

namespace TestApp.AppConfig
{
    public class DataMapConfig : BaseDataMapConfig
    {
        public static void RegisterMapper(IOC ioc)
        {
            var config = new MapperConfiguration(cfg => {
                ConfigureValueObjects(cfg);
                ConfigureCustomer(cfg);
            });

            var mapper = new Mapper(config);
            ioc.RegisterInstance<IMapper>(mapper);
        }

        private static void ConfigureValueObjects(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SimpleAddress, AddressDto>()
                .ReverseMap();
            cfg.CreateMap<SimplePersonName, PersonNameDto>()
                .ReverseMap();
            cfg.CreateMap<PhoneNumber, PhoneNumberDto>()
                .ReverseMap();
            cfg.CreateMap<PhoneNumber, PhoneNumber>();
        }

        private static void ConfigureCustomer(IMapperConfigurationExpression cfg)
        {
            MapIdentity(cfg, p => new CustomerId(p));

            cfg.CreateMap<Customer, DbCustomer>()
                .ForMember(p => p.NotInEntity, opt => opt.Ignore())
                .ReverseMap();

            cfg.CreateMap<DbCustomer, CustomerDto>();
        }
    }
}
