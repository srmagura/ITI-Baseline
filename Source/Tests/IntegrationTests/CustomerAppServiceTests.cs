using IntegrationTests.Harness;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestApp.Application;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
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
            var customerSvc = _ioc!.ResolveForTest<ICustomerAppService>();
            
            var customerId = customerSvc.Add(
                "myCustomer",
                new AddressDto {
                    Line1 = "line1",
                    Line2 = "line2",
                    City = "city",
                    State = "NC",
                    Zip = "12345"
                },
                null,
                new PhoneNumberDto { Value = "19194122710" }
            );
            Assert.IsNotNull(customerId);
            Assert.AreNotEqual(customerId, default);

            var customer = customerSvc.Get(customerId);
            Assert.IsNotNull(customer);
            Assert.AreEqual("myCustomer", customer!.Name);
            Assert.AreEqual(99, customer.SomeNumber);
            Assert.IsNull(customer.ContactName);
            Assert.AreEqual("19194122710", customer.ContactPhone?.Value);

            Assert.AreEqual("line1", customer.Address!.Line1);
            Assert.AreEqual("line2", customer.Address.Line2);
            Assert.AreEqual("city", customer.Address.City);
            Assert.AreEqual("NC", customer.Address.State);
            Assert.AreEqual("12345", customer.Address.Zip);

            //Assert.AreEqual(2, customer.LtcPharmacies.Count);
            //Assert.AreEqual("Pruitt", customer.LtcPharmacies[0].Name);
            //Assert.AreEqual("Alixa", customer.LtcPharmacies[1].Name);

            CollectionAssert.AreEqual(new List<int> { 1, 2 }, customer.SomeInts);
        }
    }
}
