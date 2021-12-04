using Autofac;
using ITI.Baseline.Audit;
using ITI.Baseline.RequestTrace;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using TestApp.Application;
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
        builder.RegisterType<NullDomainEventPublisher>().As<IDomainEventPublisher>();

        builder.RegisterModule<MapperModule>();

        builder.RegisterType<AppAuthContext>().As<IAuthContext>();
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
}
