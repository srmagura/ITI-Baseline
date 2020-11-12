using IntegrationTests.Harness;
using ITI.Baseline.Audit;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.AppConfig;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;

namespace IntegrationTests
{
    [TestClass]
    public class CustomerAuditTests
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
            var customerSvc = _ioc!.ResolveForTest<ICustomerAppService>();
            var auditAppSvc = _ioc!.ResolveForTest<IAuditAppService>();

            var customerId = AddCustomer(customerSvc);
            var auditRecords = auditAppSvc.List("Customer", customerId.ToString(), 0, 1000);
            
            var auditRecord = auditRecords.Single(r => r.Entity == "Customer");
            Assert.AreEqual(new TestAppAuthContext().UserId, auditRecord.UserId);
            Assert.AreEqual(new TestAppAuthContext().UserName, auditRecord.UserName);
            Assert.AreEqual("Customer", auditRecord.Aggregate);
            Assert.AreEqual(customerId.ToString(), auditRecord.AggregateId);
            Assert.AreEqual("Customer", auditRecord.Entity);
            Assert.AreEqual("Added", auditRecord.Event);

            var changes = JsonConvert.DeserializeObject<List<AuditPropertyDto>>(auditRecord.Changes);
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name =="Name" && p.From == null && p.To == "myCustomer")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "SomeInts" && p.From == null && p.To == "[1,2]")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.Line1" && p.From == null && p.To == "line1")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.Line2" && p.From == null && p.To == "line2")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.City" && p.From == null && p.To == "city")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.State" && p.From == null && p.To == "NC")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.Zip" && p.From == null && p.To == "12345")
            );
            Assert.IsNull(changes.FirstOrDefault(p => p.Name == "Address.HasValue"));

            var ltcAddedRecords = auditRecords.Where(r => r.Entity == "LtcPharmacy" && r.Event == "Added");
            Assert.AreEqual(2, ltcAddedRecords.Count());
        }
    }
}
