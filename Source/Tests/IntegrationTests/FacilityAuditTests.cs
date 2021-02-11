using Autofac;
using IntegrationTests.Harness;
using ITI.Baseline.Audit;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Domain.Identities;

namespace IntegrationTests
{

    [TestClass]
    public class FacilityAuditTests
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

        private FacilityId AddFacility(IFacilityAppService facilitySvc)
        {
            var facilityId = facilitySvc.Add("myFacility");
            Assert.IsNotNull(facilityId);
            Assert.AreNotEqual(facilityId, default);

            return facilityId;
        }

        [TestMethod]
        public void ChangeContact()
        {
            var facilitySvc = _container!.Resolve<IFacilityAppService>();
            var auditSvc = _container!.Resolve<IAuditAppService>();

            var facilityId = AddFacility(facilitySvc);

            facilitySvc.SetContact(facilityId,
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
            facilitySvc.SetContact(facilityId,
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

            var auditRecords = auditSvc.List("Facility", facilityId.Guid.ToString(), 0, 1000);
            var auditRecord = auditRecords.First();
            var changes = JsonConvert.DeserializeObject<List<AuditPropertyDto>>(auditRecord.Changes!);

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
