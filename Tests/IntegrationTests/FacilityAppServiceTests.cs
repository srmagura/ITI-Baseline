using Autofac;
using IntegrationTests.Harness;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Domain.Identities;

namespace IntegrationTests;

[TestClass]
public class FacilityAppServiceTests : IntegrationTest
{
    private static async Task<FacilityId> AddFacilityAsync(IFacilityAppService facilitySvc)
    {
        var facilityId = await facilitySvc.AddAsync("myFacility");
        Assert.IsNotNull(facilityId);
        Assert.AreNotEqual(facilityId, default);

        return facilityId;
    }

    [TestMethod]
    public async Task SetContactNonNullButPropertiesNull()
    {
        var facilitySvc = Container.Resolve<IFacilityAppService>();

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
        var facilitySvc = Container.Resolve<IFacilityAppService>();

        var facilityId = await AddFacilityAsync(facilitySvc);

        await facilitySvc.SetContactAsync(
            facilityId,
            new FacilityContactDto
            {
                Name = new PersonNameDto("Kelly", null, "Campbell"),
                Email = new EmailAddressDto("campbell@example2.com")
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
