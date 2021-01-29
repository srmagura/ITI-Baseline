using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ITI.Baseline.Audit;
using ITI.Baseline.Util;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using ITI.DDD.Infrastructure.DataMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.Application.Dto;
using TestApp.DataContext.DataModel;
using TestApp.Domain;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.AppConfig
{
    public class DataMapConfig : BaseDataMapConfig
    {
        public static void RegisterMapper(IOC ioc)
        {
            var config = new MapperConfiguration(cfg => {
                BaseConfig(cfg);

                ConfigureValueObjects(cfg);
                ConfigureAudit(cfg);

                ConfigureCustomer(cfg);
                ConfigureFacility(cfg);
                ConfigureUser(cfg);
            });

            config.AssertConfigurationIsValid();

            var mapper = new Mapper(config);
            ioc.RegisterInstance<IMapper>(mapper);

            SetStaticMapper(mapper);
        }

        private static void ConfigureValueObjects(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SimpleAddress, SimpleAddress>();
            cfg.CreateMap<SimpleAddress, AddressDto>();

            cfg.CreateMap<SimplePersonName, SimplePersonName>();
            cfg.CreateMap<SimplePersonName, PersonNameDto>();

            cfg.CreateMap<PhoneNumber, PhoneNumber>();
            cfg.CreateMap<PhoneNumber, PhoneNumberDto>();

            cfg.CreateMap<EmailAddress, EmailAddress>();
            cfg.CreateMap<EmailAddress, EmailAddressDto>();
        }

        private static void ConfigureAudit(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AuditRecord, AuditRecordDto>();
        }

        private static void ConfigureCustomer(IMapperConfigurationExpression cfg)
        {
            MapIdentity<CustomerId>(cfg);
            MapIdentity<LtcPharmacyId>(cfg);

            cfg.CreateMap<LtcPharmacy, DbLtcPharmacy>()
                .ForMember(p => p.Customer, opt => opt.Ignore())
                .ForMember(p => p.CustomerId, opt => opt.Ignore())
                .EqualityComparison((e, db) => e.Id.Guid == db.Id)
                .ReverseMap();

            cfg.CreateMap<Customer, DbCustomer>()
                .ForMember(p => p.SomeInts, opt => opt.Ignore())
                .AfterMap((e, db) =>
                {
                    db.SomeInts = e.SomeInts.ToDbJson();
                })
                .ReverseMap()
                .ForMember(p => p.SomeInts, opt => opt.Ignore())
                .AfterMap((db, e) =>
                {
                    SetPrivateField(e, "_someInts", db.SomeInts.FromDbJson<List<int>>());
                });

            cfg.CreateMap<DbLtcPharmacy, LtcPharmacyDto>();

            cfg.CreateMap<DbCustomer, CustomerDto>()
                .ForMember(p => p.SomeInts, opt => opt.MapFrom(src => src.SomeInts.FromDbJson<List<int>>()));
        }

        private static void ConfigureFacility(IMapperConfigurationExpression cfg)
        {
            MapIdentity<FacilityId>(cfg);

            cfg.CreateMap<FacilityContact, FacilityContact>();
            cfg.CreateMap<FacilityContact, FacilityContactDto>()
                .ReverseMap();

            cfg.CreateMap<Facility, DbFacility>()
                .ReverseMap();

            cfg.CreateMap<DbFacility, FacilityDto>();
        }

        private static void ConfigureUser(IMapperConfigurationExpression cfg)
        {
            MapIdentity<UserId>(cfg);
            MapIdentity<OnCallProviderId>(cfg);

            cfg.CreateMap<User, DbUser>()
                .IncludeAllDerived()
                .ReverseMap();

            cfg.CreateMap<CustomerUser, DbCustomerUser>()
                .ReverseMap();
            cfg.CreateMap<OnCallUser, DbOnCallUser>()
                .ReverseMap();

            cfg.CreateMap<DbUser, UserDto>()
                .IncludeAllDerived()
                .ReverseMap();

            cfg.CreateMap<DbCustomerUser, CustomerUserDto>();
            cfg.CreateMap<DbOnCallUser, OnCallUserDto>();
        }
    }
}
