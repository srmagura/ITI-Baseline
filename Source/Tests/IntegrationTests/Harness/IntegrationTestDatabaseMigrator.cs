using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.DataContext;

namespace IntegrationTests.Harness
{
    [TestClass]
    public class IntegrationTestDatabaseMigrator
    {
        [AssemblyInitialize]
        public static void MigrateDatabase(TestContext testContext)
        {
            var connectionStrings = IntegrationTestAppConfig.GetConnectionStrings(testContext);
            AppDataContext.Migrate(connectionStrings);
        }
    }
}
