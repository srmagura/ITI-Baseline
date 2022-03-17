using Autofac;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ITI.Baseline.Audit;
using ITI.Baseline.Util;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Infrastructure.DataMapping;
using TestApp.Application.Dto;
using TestApp.DataContext.DataModel;
using TestApp.Domain;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace TestApp.AppConfig;

internal class MapperModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddCollectionMappers();

            ConfigureValueObjects(cfg);
            ConfigureAudit(cfg);

            ConfigureCustomer(cfg);
            ConfigureFacility(cfg);
            ConfigureUser(cfg);
        });

        var mapper = new Mapper(config);
        builder.RegisterInstance(mapper).As<IMapper>();
    }

    private static void ConfigureValueObjects(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Address, Address>();
        cfg.CreateMap<Address, AddressDto>();

        cfg.CreateMap<PersonName, PersonName>();
        cfg.CreateMap<PersonName, PersonNameDto>();

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
        MapperConfigurationUtil.MapIdentity(cfg, guid => new CustomerId(guid));
        MapperConfigurationUtil.MapIdentity(cfg, guid => new LtcPharmacyId(guid));

        MapperConfigurationUtil.MapIdentity(cfg, guid => new VendorId(guid));

        cfg.CreateMap<LtcPharmacy, DbLtcPharmacy>(MemberList.Source)
            .ForMember(p => p.Customer, opt => opt.Ignore())
            .ForMember(p => p.CustomerId, opt => opt.Ignore())
            .EqualityComparison((e, db) => e.Id.Guid == db.Id)
            .ReverseMap();

        cfg.CreateMap<Customer, DbCustomer>(MemberList.Source)
            .ForMember(p => p.SomeInts, opt => opt.Ignore())
            .AfterMap((e, db) =>
            {
                db.SomeInts = e.SomeInts.ToDbJson();
            })
            .ReverseMap()
            .ForMember(p => p.SomeInts, opt => opt.Ignore())
#pragma warning disable CS0618 // Type or member is obsolete
            .ConstructUsing((db, ctx) => new Customer(
                placeholder: true,
                name: db.Name,
                ltcPharmacies: ctx.Mapper.Map<List<LtcPharmacy>>(db.LtcPharmacies),
                someInts: db.SomeInts.FromDbJson<List<int>>() ?? new(),
                someNumber: db.SomeNumber)
            );
#pragma warning restore CS0618 // Type or member is obsolete

        cfg.CreateMap<DbLtcPharmacy, LtcPharmacyDto>();

        cfg.CreateMap<DbCustomer, CustomerDto>()
            .ForMember(p => p.SomeInts, opt => opt.MapFrom(src => src.SomeInts.FromDbJson<List<int>>()));
    }

    private static void ConfigureFacility(IMapperConfigurationExpression cfg)
    {
        MapperConfigurationUtil.MapIdentity(cfg, guid => new FacilityId(guid));

        cfg.CreateMap<FacilityContact, FacilityContact>();
        cfg.CreateMap<FacilityContact, FacilityContactDto>()
            .ReverseMap();

        cfg.CreateMap<Facility, DbFacility>(MemberList.Source)
            .ReverseMap();

        cfg.CreateMap<DbFacility, FacilityDto>();
    }

    private static void ConfigureUser(IMapperConfigurationExpression cfg)
    {
        MapperConfigurationUtil.MapIdentity(cfg, guid => new UserId(guid));
        MapperConfigurationUtil.MapIdentity(cfg, guid => new OnCallProviderId(guid));

        cfg.CreateMap<User, DbUser>(MemberList.Source)
            .IncludeAllDerived()
            .ReverseMap();

        cfg.CreateMap<CustomerUser, DbCustomerUser>(MemberList.Source)
            .ReverseMap();
        cfg.CreateMap<OnCallUser, DbOnCallUser>(MemberList.Source)
            .ReverseMap();

        cfg.CreateMap<DbUser, UserDto>()
            .IncludeAllDerived()
            .ReverseMap();

        cfg.CreateMap<DbCustomerUser, CustomerUserDto>();
        cfg.CreateMap<DbOnCallUser, OnCallUserDto>();
    }
}
