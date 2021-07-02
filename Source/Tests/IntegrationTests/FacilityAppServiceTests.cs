using Autofac;
using IntegrationTests.Harness;
using ITI.Baseline.Audit;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Application;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Domain.Identities;
using TestApp.Domain.ValueObjects;

namespace IntegrationTests
{
    [TestClass]
    public class FacilityAppServiceTests
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

        private static async Task<FacilityId> AddFacilityAsync(IFacilityAppService facilitySvc)
        {
            var facilityId = await facilitySvc.AddAsync("myFacility");
            Assert.IsNotNull(facilityId);
            Assert.AreNotEqual(facilityId, default);

            return facilityId;
        }

        [TestMethod]
        public async Task SetContactNull()
        {
            var facilitySvc = _container!.Resolve<IFacilityAppService>();

            var facilityId = await AddFacilityAsync(facilitySvc);
            var facility = await facilitySvc.GetAsync(facilityId);

            Assert.IsNotNull(facility);
            Assert.AreEqual("myFacility", facility!.Name);
            Assert.IsNull(facility.Contact);
        }

        [TestMethod]
        public async Task SetContactNonNullButPropertiesNull()
        {
            var facilitySvc = _container!.Resolve<IFacilityAppService>();

            var facilityId = await AddFacilityAsync(facilitySvc);
            await facilitySvc.SetContactAsync(facilityId, new FacilityContactDto());

            var facility = await facilitySvc.GetAsync(facilityId);

            Assert.IsNotNull(facility);
            Assert.IsNotNull(facility!.Contact);
            Assert.IsNull(facility.Contact!.Name);
            Assert.IsNull(facility.Contact!.Email);
        }

        [TestMethod]
        public async Task SetContactNonNull()
        {
            var facilitySvc = _container!.Resolve<IFacilityAppService>();

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

            var facility = await facilitySvc.GetAsync(facilityId);

            Assert.IsNotNull(facility);
            Assert.IsNotNull(facility!.Contact);
            Assert.AreEqual("Kelly", facility.Contact!.Name?.First);
            Assert.AreEqual("Campbell", facility.Contact!.Name?.Last);
            Assert.AreEqual("campbell@example2.com", facility.Contact.Email?.Value);       
        }
    }
}
