using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.DataContext;

namespace IntegrationTests.Harness
{
    public static class IntegrationTestAppConfig
    {
        public static IOC Initialize(TestContext? testContext)
        {
            var ioc = new IOC();
            DDDAppConfig.AddRegistrations(ioc);
            DomainEvents.ClearRegistrations();
            UnitOfWorkImpl.ShouldWaitForDomainEvents(true);

            ioc.RegisterInstance(GetConnectionStrings(testContext));

            return ioc;
        }

        public static ConnectionStrings GetConnectionStrings(TestContext? testContext)
        {
            var connectionString = "Server=localhost;Database=ITIBaseline_e2e_test;Trusted_Connection=True;Connection Timeout=180;MultipleActiveResultSets=True;";

            //if (testContext.Properties.Contains("ConnectionString"))
            //    connectionString = (string)testContext.Properties["ConnectionString"];

            return new ConnectionStrings
            {
                DefaultDataContext = connectionString
            };
        }
    }
}
