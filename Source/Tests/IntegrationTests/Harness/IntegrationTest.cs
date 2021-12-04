using ITI.Baseline.RequestTrace;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.AppConfig;
using TestApp.DataContext;
using Autofac;
using System.Threading.Tasks;
using ITI.DDD.Application;

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

            var connectionStrings = GetConnectionStrings();
            builder.RegisterInstance(connectionStrings);
            builder.RegisterInstance<IDbLoggerSettings>(connectionStrings);
            builder.RegisterInstance<IDbRequestTraceSettings>(connectionStrings);
        }

        public static ConnectionStrings GetConnectionStrings()
        {
            var connectionString = $"Server=localhost;Database=ITIBaseline_e2e_test;Trusted_Connection=True;Connection Timeout=180;MultipleActiveResultSets=True;";

            return new ConnectionStrings
            {
                DefaultDataContext = connectionString
            };
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            // TODO:SAM
            // DomainEvents.ClearRegistrations();
            UnitOfWork.ShouldWaitForDomainEvents(true);
            ConsoleLogWriter.ClearErrors();

            var builder = new ContainerBuilder();
            RegisterServices(builder);
            Container = builder.Build();

            var connectionString = GetConnectionStrings().DefaultDataContext;
            await DeleteFromTablesUtil.DeleteFromTablesAsync(connectionString);
        }
    }
}
