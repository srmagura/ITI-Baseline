using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.DataContext;

namespace IntegrationTests.Harness
{
    [TestClass]
    public class AssemblyInitialize
    {
        [AssemblyInitialize]
        public static void MigrateDatabase(TestContext _)
        {
            var connectionStrings = IntegrationTest.GetConnectionStrings();
            AppDataContext.Migrate(connectionStrings);
        }
    }
}
