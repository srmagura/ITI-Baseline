using Autofac;
using IntegrationTests.Harness;
using ITI.Baseline.Audit;
using ITI.Baseline.Util;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Core.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.AppConfig;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Application.Interfaces.RepositoryInterfaces;
using TestApp.Domain;
using TestApp.Domain.Identities;

namespace IntegrationTests
{
    [TestClass]
    public class CustomerAuditTests : IntegrationTest
    {
        private static async Task<CustomerId> AddCustomerAsync(ICustomerAppService customerSvc)
        {
            var customerId = await customerSvc.AddAsync(
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

        private static void AssertDoesNotIncludeIgnoredFields(List<AuditPropertyDto> changes)
        {
            Assert.IsNull(changes.FirstOrDefault(c => c.Name == nameof(Customer.SomeNumber)));
        }

        [TestMethod]
        public async Task Add()
        {
            var customerSvc = Container!.Resolve<ICustomerAppService>();
            var auditSvc = Container!.Resolve<IAuditAppService>();

            var customerId = await AddCustomerAsync(customerSvc);
            var auditRecords = (await auditSvc.ListAsync("Customer", customerId.Guid.ToString(), 0, 1000))!.Items;

            var auditRecord = auditRecords.Single(r => r.Entity == "Customer");
            Assert.AreEqual(new TestAppAuthContext().UserId, auditRecord.UserId);
            Assert.AreEqual(new TestAppAuthContext().UserName, auditRecord.UserName);
            Assert.AreEqual("Customer", auditRecord.Aggregate);
            Assert.AreEqual(customerId.Guid.ToString(), auditRecord.AggregateId);
            Assert.AreEqual("Customer", auditRecord.Entity);
            Assert.AreEqual("Added", auditRecord.Event);

            var changes = auditRecord.Changes.FromDbJson<List<AuditPropertyDto>>()!;
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
            Assert.IsNotNull(
                changes.SingleOrDefault(c => c.Name == nameof(Customer.SomeMoney) && c.From == null && c.To == "(hidden)")
            );
            AssertDoesNotIncludeIgnoredFields(changes);

            var ltcAddedRecords = auditRecords.Where(r => r.Entity == "LtcPharmacy" && r.Event == "Added");
            Assert.AreEqual(2, ltcAddedRecords.Count());

            foreach (var ltcAddedRecord in ltcAddedRecords)
            {
                changes = ltcAddedRecord.Changes.FromDbJson<List<AuditPropertyDto>>()!;
                Assert.IsNotNull(
                    changes.SingleOrDefault(p => p.Name == "Name" && p.From == null && p.To.HasValue())
                );
            }
        }

        [TestMethod]
        public async Task ChangeProperty()
        {
            var customerSvc = Container!.Resolve<ICustomerAppService>();
            var auditSvc = Container!.Resolve<IAuditAppService>();

            var customerId = await AddCustomerAsync(customerSvc);
            var customer = await customerSvc.GetAsync(customerId);
            var pruittId = customer!.LtcPharmacies.Single(p => p.Name == "Pruitt").Id;
            await customerSvc.RenameLtcPharmacyAsync(customerId, pruittId, "Pruitt2");

            var auditRecords = (await auditSvc.ListAsync("LtcPharmacy", pruittId.Guid.ToString(), 0, 1000))!.Items;
            var auditRecord = auditRecords.Single(r => r.Entity == "LtcPharmacy" && r.Event == "Modified");

            Assert.AreEqual(new TestAppAuthContext().UserId, auditRecord.UserId);
            Assert.AreEqual(new TestAppAuthContext().UserName, auditRecord.UserName);
            Assert.AreEqual("Customer", auditRecord.Aggregate);
            Assert.AreEqual(customerId.Guid.ToString(), auditRecord.AggregateId);
            Assert.AreEqual("LtcPharmacy", auditRecord.Entity);
            Assert.AreEqual(pruittId.Guid.ToString(), auditRecord.EntityId);

            var changes = auditRecord.Changes.FromDbJson<List<AuditPropertyDto>>()!;
            Assert.AreEqual(1, changes.Count);
            Assert.IsNotNull(
                changes.SingleOrDefault(p => p.Name == "Name" && p.From == "Pruitt" && p.To == "Pruitt2")
            );
            AssertDoesNotIncludeIgnoredFields(changes);

            var customerAuditRecords = (await auditSvc.ListAsync("Customer", customerId.Guid.ToString(), 0, 1000))!.Items;
            Assert.IsNotNull(
                customerAuditRecords.SingleOrDefault(r => r.Entity == "LtcPharmacy" && r.Event == "Modified")
            );
        }

        [TestMethod]
        public async Task ChangeValueObject()
        {
            var customerSvc = Container!.Resolve<ICustomerAppService>();
            var auditSvc = Container!.Resolve<IAuditAppService>();

            var customerId = await AddCustomerAsync(customerSvc);
            var customer = await customerSvc.GetAsync(customerId);
            await customerSvc.SetContactAsync(
                customerId,
                new PersonNameDto { First = "John", Last = "Todd" },
                new PhoneNumberDto { Value = "19195551111" }
            );

            var auditRecords = (await auditSvc.ListAsync("Customer", customerId.Guid.ToString(), 0, 1000))!.Items;
            var auditRecord = auditRecords.Single(r => r.Entity == "Customer" && r.Event == "Modified");

            Assert.AreEqual(new TestAppAuthContext().UserId, auditRecord.UserId);
            Assert.AreEqual(new TestAppAuthContext().UserName, auditRecord.UserName);
            Assert.AreEqual("Customer", auditRecord.Aggregate);
            Assert.AreEqual(customerId.Guid.ToString(), auditRecord.AggregateId);
            Assert.AreEqual("Customer", auditRecord.Entity);

            var changes = auditRecord.Changes.FromDbJson<List<AuditPropertyDto>>()!;
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
        public async Task Remove()
        {
            var customerSvc = Container!.Resolve<ICustomerAppService>();
            var auditSvc = Container!.Resolve<IAuditAppService>();

            var customerId = await AddCustomerAsync(customerSvc);
            var customer = await customerSvc.GetAsync(customerId);

            foreach (var ltcPharmacy in customer!.LtcPharmacies)
            {
                await customerSvc.RemoveLtcPharmacyAsync(customerId, ltcPharmacy.Id);
            }

            await customerSvc.RemoveAsync(customerId);

            var auditRecords = (await auditSvc.ListAsync("Customer", customerId.Guid.ToString(), 0, 1000))!.Items;
            var auditRecord = auditRecords.Single(r => r.Entity == "Customer" && r.Event == "Deleted");

            var changes = auditRecord.Changes.FromDbJson<List<AuditPropertyDto>>()!;
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
            Assert.IsNotNull(
                changes.SingleOrDefault(c => c.Name == nameof(Customer.SomeMoney) && c.From == "(hidden)" && c.To == null)
            );
            AssertDoesNotIncludeIgnoredFields(changes);
        }

        [TestMethod]
        public async Task DoesNotAddRecordIfNothingChanged()
        {
            var uow = Container!.Resolve<IUnitOfWork>();
            var customerRepo = Container!.Resolve<ICustomerRepository>();
            var customerSvc = Container!.Resolve<ICustomerAppService>();
            var auditSvc = Container!.Resolve<IAuditAppService>();

            var customerId = await AddCustomerAsync(customerSvc);

            using(var scope = uow.Begin())
            {
                await customerRepo.GetAsync(customerId);
                scope.Commit();
            };

            var auditRecords =( await auditSvc.ListAsync("Customer", customerId.Guid.ToString(), 0, 1000))!.Items;
            Assert.IsNotNull(
                auditRecords.SingleOrDefault(r => r.Entity == "Customer" && r.Event == "Added")
            );
            Assert.AreEqual(
                1,
                auditRecords.Count(r => r.Entity == "Customer")
            );
        }
    }
}
