using ITI.Baseline.Audit;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Infrastructure;
using ITI.DDD.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.QueryInterfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.DataContext;
using TestApp.Domain.Events;
using TestApp.Infrastructure;
using TestApp.Queries;
using TestApp.Repositories;
using UnitTests.Mocks;

namespace TestApp.AppConfig
{
    public static class DefaultAppConfig
    {
        public static void AddRegistrations(IOC ioc)
        {
            DDDAppConfig.AddRegistrations(ioc);
            DDDInfrastructureConfig.AddRegistrations(ioc);
            BaselineAuditConfig.AddRegistrations(ioc);

            ioc.RegisterType<IAuditFieldConfiguration, AuditFieldConfiguration>();
            ioc.RegisterType<ILogWriter, DbLogWriter>();
            
            DataMapConfig.RegisterMapper(ioc);

            DomainEvents.Register<CustomerAddedEvent, CustomerAddedDomainEventHandler>();
            ioc.RegisterType<CustomerAddedDomainEventHandler>();

            ioc.RegisterType<IAuthContext, TestAppAuthContext>();
            ioc.RegisterType<IDomainEventAuthScopeResolver, DomainEventAuthScopeResolver>();
            ioc.RegisterType<IAuditAppPermissions, AppPermissions>();
            ioc.RegisterType<AppDataContext>();

            ioc.RegisterType<IAuditDataContext, AppDataContext>();

            ioc.RegisterType<ICustomerAppService, CustomerAppService>();
            ioc.RegisterType<IFacilityAppService, FacilityAppService>();
            ioc.RegisterType<IUserAppService, UserAppService>();

            ioc.RegisterType<ICustomerRepository, EfCustomerRepository>();
            ioc.RegisterType<IFacilityRepository, EfFacilityRepository>();
            ioc.RegisterType<IUserRepository, EfUserRepository>();

            ioc.RegisterType<ICustomerQueries, EfCustomerQueries>();
            ioc.RegisterType<IFacilityQueries, EfFacilityQueries>();
            ioc.RegisterType<IUserQueries, EfUserQueries>();
        }
    }
}
