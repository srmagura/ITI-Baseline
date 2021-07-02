using Autofac;
using IntegrationTests.Harness;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Interfaces;
using TestApp.DataContext;

namespace IntegrationTests
{
    [TestClass]
    public class DomainEventTests
    {
        private static TestContext? TestContext;
        private IContainer? _container;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestContext = context;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container = IntegrationTestInitialize.Initialize(TestContext).Build();
        }

        [TestMethod]
        public async Task LogWrittenWhenCustomerAdded()
        {
            var customerSvc = _container!.Resolve<ICustomerAppService>();
            await customerSvc.AddAsync("myCustomer");

            using var db = _container!.Resolve<AppDataContext>();
            var logEntries = db.LogEntries!.ToList();
            Assert.AreEqual(1, logEntries.Count);

            var logEntry = logEntries[0];
            Assert.AreEqual("info", logEntry.Level?.ToLowerInvariant());
            Assert.AreEqual("Customer added: myCustomer (by SYSTEM)", logEntry.Message);
        }
    }
}
