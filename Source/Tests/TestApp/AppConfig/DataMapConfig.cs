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
            });

            var mapper = new Mapper(config);
            ioc.RegisterInstance<IMapper>(mapper);

            SetStaticMapper(mapper);
        }

        private static void ConfigureValueObjects(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<SimpleAddress, AddressDto>()
                .ReverseMap();
            
            cfg.CreateMap<SimplePersonName, PersonNameDto>()
                .ReverseMap();
         
            cfg.CreateMap<PhoneNumber, PhoneNumberDto>()
                .ReverseMap();
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
    }
}
