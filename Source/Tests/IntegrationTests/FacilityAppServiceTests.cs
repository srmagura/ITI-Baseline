using IntegrationTests.Harness;
using ITI.Baseline.Audit;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
    public class FacilityAppServiceTests
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

        private Guid AddFacility(IFacilityAppService facilitySvc)
        {
            var facilityId = facilitySvc.Add("myFacility");
            Assert.IsNotNull(facilityId);
            Assert.AreNotEqual(facilityId, default);

            return facilityId;
        }

        [TestMethod]
        public void SetContactNull()
        {
            var facilitySvc = _ioc!.Resolve<IFacilityAppService>();

            var facilityId = AddFacility(facilitySvc);
            var facility = facilitySvc.Get(facilityId);

            Assert.IsNotNull(facility);
            Assert.AreEqual("myFacility", facility!.Name);
            Assert.IsNull(facility.Contact);
        }

        [TestMethod]
        public void SetContactNonNullButPropertiesNull()
        {
            var facilitySvc = _ioc!.Resolve<IFacilityAppService>();

            var facilityId = AddFacility(facilitySvc);
            facilitySvc.SetContact(facilityId, new FacilityContactDto());

            var facility = facilitySvc.Get(facilityId);

            Assert.IsNotNull(facility);
            Assert.IsNotNull(facility!.Contact);
            Assert.IsNull(facility.Contact!.Name);
            Assert.IsNull(facility.Contact!.Email);
        }

        [TestMethod]
        public void SetContactNonNull()
        {
            var facilitySvc = _ioc!.Resolve<IFacilityAppService>();

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

            var facility = facilitySvc.Get(facilityId);

            Assert.IsNotNull(facility);
            Assert.IsNotNull(facility!.Contact);
            Assert.AreEqual("Kelly", facility.Contact!.Name?.First);
            Assert.AreEqual("Campbell", facility.Contact!.Name?.Last);
            Assert.AreEqual("campbell@example2.com", facility.Contact.Email?.Value);       
        }
    }
}
