using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.DataContext;

namespace IntegrationTests.Harness;

[TestClass]
public static class AssemblyInitialize
{
    [AssemblyInitialize]
    public static void MigrateDatabase(TestContext _)
    {
        new AppDataContext().Database.Migrate();
    }
}
