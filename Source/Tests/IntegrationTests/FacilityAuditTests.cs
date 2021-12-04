using Autofac;
using IntegrationTests.Harness;
using ITI.Baseline.Audit;
using ITI.Baseline.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Domain.Identities;

namespace IntegrationTests
{
    [TestClass]
    public class FacilityAuditTests : IntegrationTest
    {
        private static async Task<FacilityId> AddFacilityAsync(IFacilityAppService facilitySvc)
        {
            var facilityId = await facilitySvc.AddAsync("myFacility");
            Assert.IsNotNull(facilityId);
            Assert.AreNotEqual(facilityId, default);

            return facilityId;
        }

        [TestMethod]
        public async Task ChangeContact()
        {
            var facilitySvc = Container.Resolve<IFacilityAppService>();
            var auditSvc = Container.Resolve<IAuditAppService>();

            var facilityId = await AddFacilityAsync(facilitySvc);

            await facilitySvc.SetContactAsync(facilityId,
                new FacilityContactDto
                {
                    Name = new PersonNameDto
                    {
                        First = "Kelly",
                        Last = "Campbell"
                    },
                    Email = new EmailAddressDto
                    {
                        Value = "campbell@example2.com"
                    }
                }
            );
            await facilitySvc.SetContactAsync(facilityId,
                new FacilityContactDto
                {
                    Name = new PersonNameDto
                    {
                        First = "Sam",
                        Last = "Magura"
                    },
                    Email = new EmailAddressDto
                    {
                        Value = "magura@example2.com"
                    }
                }
            );

            var list = await auditSvc.ListAsync("Facility", facilityId.Guid.ToString(), 0, 1000);
            Assert.IsTrue(list.TotalFilteredCount > 0);
            var auditRecords = list.Items;
            var auditRecord = auditRecords.First();
            var changes = auditRecord.Changes.FromDbJson<List<AuditPropertyDto>>()!;

            Assert.IsNotNull(
                changes.SingleOrDefault(c => c.Name == "Contact.Name.First" && c.From == "Kelly" && c.To == "Sam")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(c => c.Name == "Contact.Name.Last" && c.From == "Campbell" && c.To == "Magura")
            );
            Assert.IsNotNull(
                changes.SingleOrDefault(c => c.Name == "Contact.Email.Value" && c.From == "campbell@example2.com" && c.To == "magura@example2.com")
            );
        }
    }
}
