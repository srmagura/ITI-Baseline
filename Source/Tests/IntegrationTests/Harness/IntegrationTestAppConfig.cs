using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Infrastructure;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RequestTrace;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.AppConfig;
using TestApp.DataContext;

namespace IntegrationTests.Harness
{
    public static class IntegrationTestAppConfig
    {
        public static IOC Initialize(TestContext? testContext)
        {
            DomainEvents.ClearRegistrations();
            UnitOfWorkImpl.ShouldWaitForDomainEvents(true);

            var ioc = new IOC();
            DefaultAppConfig.AddRegistrations(ioc);

            var connectionStrings = GetConnectionStrings(testContext);
            ioc.RegisterInstance(connectionStrings);
            ioc.RegisterInstance<IDbLoggerSettings>(connectionStrings);
            ioc.RegisterInstance<IDbRequestTraceSettings>(connectionStrings);

            return ioc;
        }

        public static ConnectionStrings GetConnectionStrings(TestContext? testContext)
        {
            var connectionString = "Server=localhost;Database=ITIBaseline_e2e_test;Trusted_Connection=True;Connection Timeout=180;MultipleActiveResultSets=True;";

            if (testContext?.Properties.Contains("ConnectionString") ?? false)
            {
                var tmp = (string?)testContext.Properties["ConnectionString"];
                if (tmp != null) connectionString = tmp;
            }

            return new ConnectionStrings
            {
                DefaultDataContext = connectionString
            };
        }
    }
}
