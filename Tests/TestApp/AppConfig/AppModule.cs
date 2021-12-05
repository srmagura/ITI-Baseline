using Autofac;
using ITI.Baseline.Audit;
using ITI.Baseline.RequestTracing;
using ITI.DDD.Application;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Application.DomainEvents.Direct;
using ITI.DDD.Auth;
using TestApp.Application;
using TestApp.Application.EventHandlers;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.DataContext;
using TestApp.Queries;
using TestApp.Repositories;

namespace TestApp.AppConfig;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterModule<ITIDDDModule>();
        builder.RegisterModule<ITIAuditModule>();
        builder.RegisterType<AuditFieldConfiguration>().As<IAuditFieldConfiguration>();
        builder.RegisterType<DbRequestTrace>().As<IRequestTrace>();

        RegisterDomainEventPlumbing(builder);
        RegisterDomainEventHandlers(builder);

        builder.RegisterModule<MapperModule>();

        builder.RegisterType<AppAuthContext>().As<IAuthContext>();
        builder.RegisterType<AppPermissions>().As<IAuditAppPermissions>();
        builder.RegisterType<AppDataContext>().AsSelf().As<IAuditDataContext>();

        builder.RegisterType<CustomerAppService>().As<ICustomerAppService>();
        builder.RegisterType<FacilityAppService>().As<IFacilityAppService>();
        builder.RegisterType<UserAppService>().As<IUserAppService>();

        builder.RegisterType<EfCustomerRepository>().As<ICustomerRepository>();
        builder.RegisterType<EfFacilityRepository>().As<IFacilityRepository>();
        builder.RegisterType<EfUserRepository>().As<IUserRepository>();

        builder.RegisterType<EfCustomerQueries>().As<ICustomerQueries>();
        builder.RegisterType<EfFacilityQueries>().As<IFacilityQueries>();
        builder.RegisterType<EfUserQueries>().As<IUserQueries>();
    }

    private static void RegisterDomainEventPlumbing(ContainerBuilder builder)
    {
        builder.RegisterType<DirectDomainEventPublisher>().As<IDomainEventPublisher>();

        builder.RegisterType<DirectDomainEventPublisherLifetimeScopeProvider>()
            .As<IDirectDomainEventPublisherLifetimeScopeProvider>();

        builder.RegisterBuildCallback(DirectDomainEventPublisherLifetimeScopeProvider.OnContainerBuilt);
    }

    private static void RegisterDomainEventHandlers(ContainerBuilder builder)
    {
        var registryBuilder = new DomainEventHandlerRegistryBuilder(builder);
        CustomerEventHandler.Register(registryBuilder);

        builder.RegisterInstance(registryBuilder.Build());
    }
}
