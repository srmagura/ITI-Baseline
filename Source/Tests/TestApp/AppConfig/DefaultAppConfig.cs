using ITI.DDD.Application;
using ITI.DDD.Core;
using ITI.DDD.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Application;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.Repositories;

namespace TestApp.AppConfig
{
    public static class DefaultAppConfig
    {
        public static IOC Initialize()
        {
            var ioc = new IOC();
            DDDAppConfig.AddRegistrations(ioc);
            DDDInfrastructureConfig.AddRegistrations(ioc);

            ioc.RegisterType<ICustomerAppService, CustomerAppService>();

            ioc.RegisterType<ICustomerRepository, EfCustomerRepository>();

            return ioc;
        }
    }
}
