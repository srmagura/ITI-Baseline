using IntegrationTests.Harness;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private Guid AddCustomer(ICustomerAppService customerSvc)
        {
            var customerId = customerSvc.Add(
                "myCustomer",
                new AddressDto
                {
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

            return customerId;
        }

        [TestMethod]
        public void Add()
        {
            var customerSvc = _ioc!.Resolve<ICustomerAppService>();

            var customerId = AddCustomer(customerSvc);
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

            CollectionAssert.AreEquivalent(
                new List<string> { "Pruitt", "Alixa" },
                customer.LtcPharmacies.Select(p => p.Name).ToList()
            );

            CollectionAssert.AreEqual(new List<int> { 1, 2 }, customer.SomeInts);
        }

        [TestMethod]
        public void SetContact()
        {
            var customerSvc = _ioc!.Resolve<ICustomerAppService>();

            var customerId = AddCustomer(customerSvc);
            
            var customer = customerSvc.Get(customerId);
            Assert.IsNull(customer!.ContactName);
            Assert.AreEqual("19194122710", customer.ContactPhone?.Value);

            customerSvc.SetContact(
                customerId,
                new PersonNameDto { First = "Sam", Last = "Magura" },
                new PhoneNumberDto { Value = "19195551111" }
            );
            customer = customerSvc.Get(customerId);
            Assert.AreEqual("Sam", customer!.ContactName?.First);
            Assert.AreEqual("Magura", customer.ContactName?.Last);
            Assert.AreEqual("19195551111", customer.ContactPhone?.Value);

            customerSvc.SetContact(customerId, null, null);
            customer = customerSvc.Get(customerId);
            Assert.IsNull(customer!.ContactName);
            Assert.IsNull(customer.ContactPhone);
        }

        [TestMethod]
        public void Remove()
        {
            var customerSvc = _ioc!.Resolve<ICustomerAppService>();

            var customerId = AddCustomer(customerSvc);
            var customer = customerSvc.Get(customerId);
            Assert.IsNotNull(customer);

            customerSvc.Remove(customerId);
            customer = customerSvc.Get(customerId);
            Assert.IsNull(customer);
        }

        [TestMethod]
        public void AddLtcPharmacy()
        {
            var customerSvc = _ioc!.Resolve<ICustomerAppService>();

            var customerId = AddCustomer(customerSvc);
            customerSvc.AddLtcPharmacy(customerId, "1st Choice");

            var customer = customerSvc.Get(customerId);
            CollectionAssert.AreEquivalent(
                new List<string> { "Pruitt", "Alixa", "1st Choice" },
                customer!.LtcPharmacies.Select(p => p.Name).ToList()
            );
        }

        [TestMethod]
        public void RenameLtcPharmacy()
        {
            var customerSvc = _ioc!.Resolve<ICustomerAppService>();

            var customerId = AddCustomer(customerSvc);
            var customer = customerSvc.Get(customerId);
            var pruittId = customer!.LtcPharmacies.Single(p => p.Name == "Pruitt").Id;
            
            customerSvc.RenameLtcPharmacy(customerId, pruittId, "Pruitt2");

            customer = customerSvc.Get(customerId);
            CollectionAssert.AreEquivalent(
                new List<string> { "Pruitt2", "Alixa" },
                customer!.LtcPharmacies.Select(p => p.Name).ToList()
            );
        }

        [TestMethod]
        public void RemoveLtcPharmacy()
        {
            var customerSvc = _ioc!.Resolve<ICustomerAppService>();

            var customerId = AddCustomer(customerSvc);
            var customer = customerSvc.Get(customerId);
            var pruittId = customer!.LtcPharmacies.Single(p => p.Name == "Pruitt").Id;

            customerSvc.RemoveLtcPharmacy(customerId, pruittId);

            customer = customerSvc.Get(customerId);
            CollectionAssert.AreEquivalent(
                new List<string> { "Alixa" },
                customer!.LtcPharmacies.Select(p => p.Name).ToList()
            );
        }
    }
}
