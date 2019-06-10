using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataContext;
using Domain;
using Iti.Core.Mapping;
using Iti.Core.ValueObjects;
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

                DefaultValueObjectMap.Configure(cfg);
                cfg.CreateMap<ValueChild, ValueChild>();
                cfg.CreateMap<ValueParent, ValueParent>();

                FooConfig(cfg);
                BarConfig(cfg);
                FooDtoConfigs(cfg);

                ValObjHolderConfig(cfg);

                //

                // depricated: ConfigureDtoValueObjects(cfg);
                ConfigureDbEntityValueObjects(cfg);
            });

            Mapper.AssertConfigurationIsValid();
        }

        private void ValObjHolderConfig(IMapperConfigurationExpression cfg)
        {
            MapIdentity(cfg, guid => new ValObjHolderId(guid));

            cfg.CreateMap<ValObjHolder, DbValObjHolder>();

            cfg.CreateMap<DbValObjHolder, ValObjHolderDto>();
        }


        private static void FooDtoConfigs(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<string, List<int>>()
                .ConvertUsing(s => ToList(s, int.Parse));

            cfg.CreateMap<DbFoo, FooReferenceDto>()
                .ConvertUsing(p => new FooReferenceDto
                {
                    Id = new FooId(p.Id),
                    Name = p.Name,
                });

            cfg.CreateMap<DbFoo, FooSummaryDto>()
                .ConvertUsing(p => new FooSummaryDto
                {
                    Id = new FooId(p.Id),
                    Name = p.Name,
                    BarCount = p.Bars.Count,
                    SomeInts = p.SomeInts,
                })
                ;

            cfg.CreateMap<DbFoo, FooDto>()
                .ForMember(p => p.SomeInts, opt => opt.MapFrom(src => src.SomeInts.Split(',').Select(int.Parse).ToList()))
                .ConvertUsing(foo => new FooDto
                {
                    Id = new FooId(foo.Id),
                    Name = foo.Name,
                    SomeMoney = foo.SomeMoney,
                    Address = foo.Address.NullIfNoValue(),
                    PhoneNumber = foo.PhoneNumber.NullIfNoValue(),
                    PersonName = foo.PersonName.NullIfNoValue(),
                    SomeNumber = foo.SomeNumber,
                    Bars = Mapper.Map<List<BarDto>>(foo.Bars),
                    SomeInts = foo.SomeInts,
                    // SomeInts = foo.SomeInts.Split(',').Select(int.Parse).ToList()
                });

            cfg.CreateMap<DbFoo, FooJunkDto>()
                .ForMember(p => p.Ref, opt => opt.MapFrom(p => p))
                .ForMember(p => p.Summary, opt => opt.MapFrom(p => p))
                ;
        }

        private void BarConfig(IMapperConfigurationExpression cfg)
        {
            MapIdentity(cfg, p => new BarId(p));

            cfg.CreateMap<Bar, DbBar>()
                // .ForMember(p => p.FooId, opt => opt.Ignore())
                // .ForMember(p => p.Foo, opt => opt.Ignore())
                .ForMember(p => p.NotInEntity, opt => opt.Ignore())
                .AfterMap((e, db) =>
                {
                })
                .ReverseMap()
                .AfterMap((db, e) =>
                {
                })
                ;
        }

        private void FooConfig(IMapperConfigurationExpression cfg)
        {
            MapIdentity(cfg, p => new FooId(p));

            cfg.CreateMap<Foo, DbFoo>()
                .ForMember(p => p.NotInEntity, opt => opt.Ignore())
                .ForMember(p => p.Bars, opt => opt.Ignore())
                .ForMember(p => p.SomeInts, opt => opt.Ignore())
                .ForMember(p => p.SomeGuids, opt => opt.Ignore())
                .AfterMap((e, db) =>
                {
                    db.Bars = MapCollection(e.Bars, db.Bars);
                    db.SomeInts = string.Join(",", e.SomeInts);
                    db.SomeGuids = string.Join("|", e.SomeGuids);
                })
                .ReverseMap()
                .AfterMap((db, e) =>
                {
                    // SetPrivateField(e, "_bars", Mapper.Map<List<Bar>>(db.Bars));
                    SetPrivateField(e, "_bars", db.Bars.Select(p => p.ToEntity<Bar>()).ToList());

                    SetPrivateField(e, "_someInts", ToList(db.SomeInts, int.Parse));
                    SetPrivateField(e, "_someGuids", ToList(db.SomeGuids, Guid.Parse));
                })
                ;
        }


    }
}
