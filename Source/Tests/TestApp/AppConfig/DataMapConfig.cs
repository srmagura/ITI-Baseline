﻿using AutoMapper;
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
                ConfigureCustomer(cfg);
            });

            var mapper = new Mapper(config);
            ioc.RegisterInstance<IMapper>(mapper);

            Mapper = mapper;
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

        private static void ConfigureCustomer(IMapperConfigurationExpression cfg)
        {
            MapIdentity(cfg, p => new CustomerId(p));
            MapIdentity(cfg, p => new LtcPharmacyId(p));

            cfg.CreateMap<LtcPharmacy, DbLtcPharmacy>()
                .ReverseMap();

            cfg.CreateMap<Customer, DbCustomer>()
                .ForMember(p => p.NotInEntity, opt => opt.Ignore())
                .ForMember(p => p.LtcPharmacies, opt => opt.Ignore())
                .ForMember(p => p.SomeInts, opt => opt.Ignore())
                .AfterMap((e, db) =>
                {
                    db.LtcPharmacies = MapCollection(e.LtcPharmacies, db.LtcPharmacies);
                    db.SomeInts = e.SomeInts.ToDbJson();
                })
                .ReverseMap()
                .ForMember(p => p.LtcPharmacies, opt => opt.Ignore())
                .ForMember(p => p.SomeInts, opt => opt.Ignore())
                .AfterMap((db, e) =>
                {
                    //SetPrivateField(e, "_ltcPharmacies", db.LtcPharmacies.Select(p => p.ToEntity<LtcPharmacy>()).ToList());
                    SetPrivateField(e, "_someInts", db.SomeInts.FromDbJson<List<int>>());
                });

            cfg.CreateMap<DbLtcPharmacy, LtcPharmacyDto>();

            cfg.CreateMap<DbCustomer, CustomerDto>()
                .ForMember(p => p.SomeInts, opt => opt.MapFrom(src => src.SomeInts.FromDbJson<List<int>>()));
        }
    }
}