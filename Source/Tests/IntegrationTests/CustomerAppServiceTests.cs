using IntegrationTests.Harness;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestApp.Application;
using TestApp.Domain.ValueObjects;

namespace IntegrationTests
{
    [TestClass]
    public class CustomerAppServiceTests
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
        public void Add()
        {
            var customerSvc = _ioc!.ResolveForTest<CustomerAppService>();
            
            var customerId = customerSvc.Add(
                "myCustomer",
                new SimpleAddress("line1", "line2", "city", "NC", "12345"),
                null,
                new PhoneNumber("19194122710")
            );
            Assert.IsNotNull(customerId);
            Assert.AreNotEqual(customerId!.Guid, default);

            var customer = customerSvc.Get(customerId);
            Assert.AreEqual("myCustomer", customer.Name);
            Assert.AreEqual("line1", customer.Address.Line1);
            Assert.AreEqual("line2", customer.Address.Line2);
            Assert.AreEqual("city", customer.Address.City);
            Assert.AreEqual("NC", customer.Address.State);
            Assert.AreEqual("12345", customer.Address.Zip);
            Assert.IsNull(customer.ContactName);
            Assert.AreEqual(customer.ContactPhone?.Value, "19194122710");
        }
    }
}
