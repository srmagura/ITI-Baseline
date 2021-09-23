using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Domain.DomainEvents;
using ITI.Baseline.RequestTrace;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.AppConfig;
using TestApp.DataContext;
using Autofac;
using System.Threading.Tasks;

namespace IntegrationTests.Harness
{
    public abstract class IntegrationTest
    {
        protected const string TestDatabaseName = "ITIBaseline_e2e_test";

        protected static TestContext? TestContext;

        protected IContainer? Container { get; set; }

        // NO! This method won't be called if defined on the base class
        //[ClassInitialize]
        //public static void ClassInitialize(TestContext context)
        //{
        //    _testContext = context;
        //}

        [TestCleanup]
        public void TestCleanup()
        {
            Assert.IsFalse(ConsoleLogWriter.HasErrors);
        }

        protected static void RegisterServices(ContainerBuilder builder)
        {
            DefaultAppConfig.AddRegistrations(builder);

            builder.RegisterType<ConsoleLogWriter>().As<ILogWriter>();

            var connectionStrings = GetConnectionStrings(TestContext);
            builder.RegisterInstance(connectionStrings);
            builder.RegisterInstance<IDbLoggerSettings>(connectionStrings);
            builder.RegisterInstance<IDbRequestTraceSettings>(connectionStrings);
        }

        public static ConnectionStrings GetConnectionStrings(TestContext? testContext)
        {
            var connectionString = $"Server=localhost;Database={TestDatabaseName};Trusted_Connection=True;Connection Timeout=180;MultipleActiveResultSets=True;";

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

        [TestInitialize]
        public async Task TestInitialize()
        {
            DomainEvents.ClearRegistrations();
            UnitOfWorkImpl.ShouldWaitForDomainEvents(true);
            ConsoleLogWriter.ClearErrors();

            var builder = new ContainerBuilder();
            RegisterServices(builder);
            Container = builder.Build();

            var connectionString = GetConnectionStrings(TestContext).DefaultDataContext;
            await DeleteFromTablesUtil.DeleteFromTablesAsync(connectionString);
        }
    }
}
