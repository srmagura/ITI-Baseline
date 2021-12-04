using ITI.Baseline.RequestTrace;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.AppConfig;
using TestApp.DataContext;
using Autofac;

namespace IntegrationTests.Harness
{
    public abstract class IntegrationTest
    {
#nullable disable // To avoid unnecessary null checks in every test
        protected IContainer Container { get; set; }
#nullable enable

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(ConsoleLogWriter.HasErrors);
        }

        protected static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterModule<AppModule>();

            builder.RegisterType<ConsoleLogWriter>().As<ILogWriter>();

            var connectionStrings = new ConnectionStrings();
            builder.RegisterInstance(connectionStrings);
            builder.RegisterInstance<IDbLoggerSettings>(connectionStrings);
            builder.RegisterInstance<IDbRequestTraceSettings>(connectionStrings);
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            ConsoleLogWriter.ClearErrors();

            var builder = new ContainerBuilder();
            RegisterServices(builder);
            Container = builder.Build();

            await DeleteFromTablesUtil.DeleteFromTablesAsync(new ConnectionStrings().AppDataContext);
        }
    }
}
