using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataContext;
using Domain;
using Iti.Core.Mapping;
using Iti.Core.ValueObjects;
using Iti.Passwords;
using Iti.ValueObjects;
using SampleApp.Application.Dto;

namespace AppConfig
{
    public class DataMapConfig : BaseDataMapConfig
    {
        public static void Initialize()
        {
            new DataMapConfig()._Initialize();
        }

        private static bool _isInit;
        private void _Initialize()
        {
            if (_isInit)
                return;
            _isInit = true;

            Mapper.Initialize(cfg =>
            {
                cfg.DisableConstructorMapping();
                cfg.Advanced.AllowAdditiveTypeMapCreation = true;

                cfg.AddGlobalIgnore("MappedEntity");

                //ValueObjectConfigs(cfg);
                DefaultValueObjectMap.Configure(cfg);

                FooConfig(cfg);
                BarConfig(cfg);
                FooDtoConfigs(cfg);
            });

            Mapper.AssertConfigurationIsValid();
        }

        /*
        private static void ValueObjectConfigs(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TimeZone, TimeZone>();
            cfg.CreateMap<Address, Address>();
            cfg.CreateMap<EmailAddress, EmailAddress>();
            cfg.CreateMap<PersonName, PersonName>();
            cfg.CreateMap<PhoneNumber, PhoneNumber>();
            cfg.CreateMap<EncodedPassword, EncodedPassword>();
        }
        */

        private static void FooDtoConfigs(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<string, List<int>>()
                .ConvertUsing(s => ToList(s, int.Parse));

            cfg.CreateMap<DbFoo, FooReferenceDto>()
                .ProjectUsing(p => new FooReferenceDto
                {
                    Id = new FooId(p.Id),
                    Name = p.Name,
                });

            cfg.CreateMap<DbFoo, FooSummaryDto>()
                .ProjectUsing(p => new FooSummaryDto
                {
                    Id = new FooId(p.Id),
                    Name = p.Name,
                    BarCount = p.Bars.Count,
                    SomeInts = p.SomeInts,
                })
                ;

            cfg.CreateMap<DbFoo, FooDto>()
                .ConvertUsing(foo => new FooDto
                {
                    Id = new FooId(foo.Id),
                    Name = foo.Name,
                    SomeMoney = foo.SomeMoney,
                    Address = foo.Address.NullIfNoValue(),
                    SomeNumber = foo.SomeNumber,
                    Bars = Mapper.Map<List<BarDto>>(foo.Bars),
                    SomeInts = foo.SomeInts.Split(',').Select(int.Parse).ToList()
                });

            cfg.CreateMap<DbFoo, FooJunkDto>()
                .ForMember(p => p.Ref, opt => opt.MapFrom(p => p))
                .ForMember(p => p.Summary, opt => opt.MapFrom(p => p))
                ;
        }

        private void BarConfig(IMapperConfigurationExpression cfg)
        {
            MapIdentity<BarId>(cfg);

            cfg.CreateMap<Bar, DbBar>()
                .ForMember(p => p.FooId, opt => opt.Ignore())
                .ForMember(p => p.Foo, opt => opt.Ignore())
                .ForMember(p => p.NotInEntity, opt => opt.Ignore())
                .ReverseMap()
                ;
        }

        private void FooConfig(IMapperConfigurationExpression cfg)
        {
            MapIdentity<FooId>(cfg);

            cfg.CreateMap<Foo, DbFoo>()

                .ForMember(p => p.NotInEntity, opt => opt.Ignore())
                .ForMember(p => p.Address, opt => opt.Ignore())
                .ForMember(p => p.Bars, opt => opt.Ignore())
                .ForMember(p => p.SomeInts, opt => opt.Ignore())
                .ForMember(p => p.SomeGuids, opt => opt.Ignore())

                .AfterMap((e, db) =>
                {
                    db.Address = MapValueObject(e.Address, db.Address); //e.Address ?? CreateInstance(db.Address);
                    db.Bars = MapCollection(e.Bars, db.Bars);
                    db.SomeInts = string.Join(",", e.SomeInts);
                    db.SomeGuids = string.Join("|", e.SomeGuids);
                })

                .ReverseMap()

                .AfterMap((db, e) =>
                {
                    SetPrivateField(e, "Address", db.Address.NullIfNoValue());
                    SetPrivateField(e, "_bars", Mapper.Map<List<Bar>>(db.Bars));
                    SetPrivateField(e, "_someInts", ToList(db.SomeInts, int.Parse));
                    SetPrivateField(e, "_someGuids", ToList(db.SomeGuids, Guid.Parse));
                })
                ;
        }


    }
}
