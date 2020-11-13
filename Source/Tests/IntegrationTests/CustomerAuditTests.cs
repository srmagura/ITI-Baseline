using IntegrationTests.Harness;
using ITI.Baseline.Audit;
using ITI.DDD.Core;
using ITI.DDD.Core.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.AppConfig;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.DataContext;
using TestApp.Domain;

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
                new PersonNameDto { First = "Sam", Last = "Magura" },
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
            var auditSvc = _ioc!.ResolveForTest<IAuditAppService>();

            var customerId = AddCustomer(customerSvc);
            var auditRecords = auditSvc.List("Customer", customerId.ToString(), 0, 1000);

            var auditRecord = auditRecords.Single(r => r.Entity == "Customer");
            Assert.AreEqual(new TestAppAuthContext().UserId, auditRecord.UserId);
            Assert.AreEqual(new TestAppAuthContext().UserName, auditRecord.UserName);
            Assert.AreEqual("Customer", auditRecord.Aggregate);
            Assert.AreEqual(customerId.ToString(), auditRecord.AggregateId);
            Assert.AreEqual("Customer", auditRecord.Entity);
            Assert.AreEqual("Added", auditRecord.Event);

            var changes = JsonConvert.DeserializeObject<List<AuditPropertyDto>>(auditRecord.Changes);
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Name" && p.From == null && p.To == "myCustomer")
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

            foreach (var ltcAddedRecord in ltcAddedRecords)
            {
                changes = JsonConvert.DeserializeObject<List<AuditPropertyDto>>(auditRecord.Changes);
                Assert.IsNotNull(
                    changes.SingleOrDefault(p => p.Name == "Name" && p.From == null && p.To.HasValue())
                );
            }
        }

        [TestMethod]
        public void ChangeProperty()
        {
            var customerSvc = _ioc!.ResolveForTest<ICustomerAppService>();
            var auditSvc = _ioc!.ResolveForTest<IAuditAppService>();

            var customerId = AddCustomer(customerSvc);
            var customer = customerSvc.Get(customerId);
            var pruittId = customer!.LtcPharmacies.Single(p => p.Name == "Pruitt").Id;
            customerSvc.RenameLtcPharmacy(customerId, pruittId, "Pruitt2");

            var auditRecords = auditSvc.List("LtcPharmacy", pruittId.ToString(), 0, 1000);
            var auditRecord = auditRecords.Single(r => r.Entity == "LtcPharmacy" && r.Event == "Modified");

            Assert.AreEqual(new TestAppAuthContext().UserId, auditRecord.UserId);
            Assert.AreEqual(new TestAppAuthContext().UserName, auditRecord.UserName);
            Assert.AreEqual("Customer", auditRecord.Aggregate);
            Assert.AreEqual(customerId.ToString(), auditRecord.AggregateId);
            Assert.AreEqual("LtcPharmacy", auditRecord.Entity);
            Assert.AreEqual(pruittId.ToString(), auditRecord.EntityId);

            var changes = JsonConvert.DeserializeObject<List<AuditPropertyDto>>(auditRecord.Changes);
            Assert.AreEqual(1, changes.Count);
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Name" && p.From == "Pruitt" && p.To == "Pruitt2")
            );
        }

        [TestMethod]
        public void ChangeValueObject()
        {
            var customerSvc = _ioc!.ResolveForTest<ICustomerAppService>();
            var auditSvc = _ioc!.ResolveForTest<IAuditAppService>();

            var customerId = AddCustomer(customerSvc);
            var customer = customerSvc.Get(customerId);
            customerSvc.SetContact(
                customerId,
                new PersonNameDto { First = "John", Last = "Todd" },
                new PhoneNumberDto { Value = "19195551111" }
            );

            var auditRecords = auditSvc.List("Customer", customerId.ToString(), 0, 1000);
            var auditRecord = auditRecords.Single(r => r.Entity == "Customer" && r.Event == "Modified");

            Assert.AreEqual(new TestAppAuthContext().UserId, auditRecord.UserId);
            Assert.AreEqual(new TestAppAuthContext().UserName, auditRecord.UserName);
            Assert.AreEqual("Customer", auditRecord.Aggregate);
            Assert.AreEqual(customerId.ToString(), auditRecord.AggregateId);
            Assert.AreEqual("Customer", auditRecord.Entity);

            var changes = JsonConvert.DeserializeObject<List<AuditPropertyDto>>(auditRecord.Changes);
            Assert.IsNull(changes.SingleOrDefault(p => p.Name == "Name"));
            Assert.IsNull(changes.SingleOrDefault(p => p.Name == "Address.Line1"));
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "ContactName.First" && p.From == "Sam" && p.To == "John")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "ContactName.Last" && p.From == "Magura" && p.To == "Todd")
            );
            Assert.IsNull(changes.FirstOrDefault(p => p.Name == "ContactName.HasValue"));
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "ContactPhone.Value" && p.From == "19194122710" && p.To == "19195551111")
            );
        }

        [TestMethod]
        public void Remove()
        {
            var customerSvc = _ioc!.ResolveForTest<ICustomerAppService>();
            var auditSvc = _ioc!.ResolveForTest<IAuditAppService>();

            var customerId = AddCustomer(customerSvc);
            var customer = customerSvc.Get(customerId);

            foreach (var ltcPharmacy in customer!.LtcPharmacies)
            {
                customerSvc.RemoveLtcPharmacy(customerId, ltcPharmacy.Id);
            }

            customerSvc.Remove(customerId);

            var auditRecords = auditSvc.List("Customer", customerId.ToString(), 0, 1000);
            var auditRecord = auditRecords.Single(r => r.Entity == "Customer" && r.Event == "Deleted");

            var changes = JsonConvert.DeserializeObject<List<AuditPropertyDto>>(auditRecord.Changes);
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Name" && p.From == "myCustomer" && p.To == null)
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "SomeInts" && p.From == "[1,2]" && p.To == null)
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.Line1" && p.From == "line1" && p.To == null)
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.Line2" && p.From == "line2" && p.To == null)
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.City" && p.From == "city" && p.To == null)
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.State" && p.From == "NC" && p.To == null)
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Address.Zip" && p.From == "12345" && p.To == null)
            );
            Assert.IsNull(changes.FirstOrDefault(p => p.Name == "Address.HasValue"));
        }
    }
}
