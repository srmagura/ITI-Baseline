using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Harness
{
    public static class IntegrationTestInitialize
    {
        public static IOC Initialize(TestContext? testContext)
        {
            var ioc = IntegrationTestAppConfig.Initialize(testContext);

            var connectionString = IntegrationTestAppConfig.GetConnectionStrings(testContext).DefaultDataContext;
            IntegrationTestData.ResetDb(connectionString);

            return ioc;
        }
    }
}
