using Autofac;
using IntegrationTests.Harness;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Application.Interfaces;
using TestApp.DataContext;

namespace IntegrationTests
{
    [TestClass]
    public class DomainEventTests : IntegrationTest
    {
        [TestMethod]
        public async Task ItLogsWhenCustomerAdded()
        {
            var builder = new ContainerBuilder();
            RegisterServices(builder);
            builder.RegisterType<DbLogWriter>().As<ILogWriter>();
            Container = builder.Build();

            var customerSvc = Container!.Resolve<ICustomerAppService>();
            await customerSvc.AddAsync("myCustomer");

            using var db = Container!.Resolve<AppDataContext>();
            var logEntries = db.LogEntries!.ToList();
            Assert.AreEqual(1, logEntries.Count);

            var logEntry = logEntries[0];
            Assert.AreEqual("info", logEntry.Level?.ToLowerInvariant());
            Assert.AreEqual("Customer added: myCustomer (by SYSTEM)", logEntry.Message);
        }
    }
}
