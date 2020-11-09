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
                changes.SingleOrDefault(p => p.Name =="Name" && p.To == "" && p.From == "myCustomer")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "SomeInts" && p.To == "" && p.From == "[1, 2]")
            );

            var ltcAddedRecords = auditRecords.Where(r => r.Entity == "LtcPharmacy" && r.Event == "Added");
            Assert.AreEqual(2, ltcAddedRecords.Count());
        }
    }
}
