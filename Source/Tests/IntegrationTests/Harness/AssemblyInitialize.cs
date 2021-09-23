using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.DataContext;

namespace IntegrationTests.Harness
{
    [TestClass]
    public class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void MigrateDatabase(TestContext testContext)
        {
            var connectionStrings = IntegrationTest.GetConnectionStrings(testContext);
            AppDataContext.Migrate(connectionStrings);
        }
    }
}
