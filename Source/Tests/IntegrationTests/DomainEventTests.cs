using IntegrationTests.Harness;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.Application.Interfaces;
using TestApp.DataContext;

namespace IntegrationTests
{
    [TestClass]
    public class DomainEventTests
    {
        private static TestContext? TestContext;
        private IOC? _ioc;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestContext = context;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _ioc = IntegrationTestInitialize.Initialize(TestContext);
        }

        [TestMethod]
        public void LogWrittenWhenCustomerAdded()
        {
            var customerSvc = _ioc!.Resolve<ICustomerAppService>();
            customerSvc.Add("myCustomer");

            using(var db = _ioc.Resolve<AppDataContext>())
            {
                var logEntries = db.LogEntries!.ToList();
                Assert.AreEqual(1, logEntries.Count);

                var logEntry = logEntries[0];
                Assert.AreEqual("info", logEntry.Level?.ToLowerInvariant());
                Assert.AreEqual("Customer added: myCustomer (by SYSTEM)", logEntry.Message);
            }
        }
    }
}
