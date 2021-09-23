using Autofac;
using ITI.Baseline.Audit;
using ITI.Baseline.RequestTrace;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Infrastructure;
using ITI.DDD.Logging;
using TestApp.Application;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.DataContext;
using TestApp.Infrastructure;
using TestApp.Queries;
using TestApp.Repositories;
using UnitTests.Mocks;

namespace TestApp.AppConfig
{
    public static class DefaultAppConfig
    {
        public static void AddRegistrations(ContainerBuilder builder)
        {
            DDDAppConfig.AddRegistrations(builder);
            DDDInfrastructureConfig.AddRegistrations(builder);
            BaselineAuditConfig.AddRegistrations(builder);

            builder.RegisterType<AuditFieldConfiguration>().As<IAuditFieldConfiguration>();
            builder.RegisterType<DapperRequestTrace>().As<IRequestTrace>();

            DataMapConfig.RegisterMapper(builder);

            ConfigureDomainEvents(builder);

            builder.RegisterType<TestAppAuthContext>().As<IAuthContext>();
            builder.RegisterType<DomainEventAuthScopeResolver>().As<IDomainEventAuthScopeResolver>();
            builder.RegisterType<AppPermissions>().As<IAuditAppPermissions>();
            builder.RegisterType<AppDataContext>();

            builder.RegisterType<AppDataContext>().As<IAuditDataContext>();

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

        private static void ConfigureDomainEvents(ContainerBuilder builder)
        {
            builder.RegisterBuildCallback(DomainEventAuthScopeResolver.OnContainerBuilt);

            CustomerAddedDomainEventHandler.Register();
            builder.RegisterType<CustomerAddedDomainEventHandler>();
        }
    }
}
