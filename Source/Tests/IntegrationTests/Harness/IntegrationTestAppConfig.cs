using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using ITI.Baseline.RequestTrace;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.AppConfig;
using TestApp.DataContext;
using Autofac;

namespace IntegrationTests.Harness
{
    public static class IntegrationTestAppConfig
    {
        public static ContainerBuilder Initialize(TestContext? testContext)
        {
            DomainEvents.ClearRegistrations();
            UnitOfWorkImpl.ShouldWaitForDomainEvents(true);

            var builder = new ContainerBuilder();
            DefaultAppConfig.AddRegistrations(builder);

            var connectionStrings = GetConnectionStrings(testContext);
            builder.RegisterInstance(connectionStrings);
            builder.RegisterInstance<IDbLoggerSettings>(connectionStrings);
            builder.RegisterInstance<IDbRequestTraceSettings>(connectionStrings);

            return builder;
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
