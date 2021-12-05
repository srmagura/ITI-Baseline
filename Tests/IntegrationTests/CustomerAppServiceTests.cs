using Autofac;
using IntegrationTests.Harness;
using ITI.DDD.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Domain.Identities;

namespace IntegrationTests;

[TestClass]
public class CustomerAppServiceTests : IntegrationTest
{
    private static async Task<CustomerId> AddCustomerAsync(ICustomerAppService customerSvc)
    {
        var customerId = await customerSvc.AddAsync(
            "myCustomer",
            new AddressDto("line1", "line2", "city", "NC", "12345"),
            null,
            new PhoneNumberDto("19194122710")
        );
        Assert.IsNotNull(customerId);
        Assert.AreNotEqual(customerId, default);

        return customerId;
    }

    [TestMethod]
    public async Task Add()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await AddCustomerAsync(customerSvc);

        var customer = await customerSvc.GetAsync(customerId);

        Assert.AreEqual(customerId, customer?.Id);

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
    public async Task SetName()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await AddCustomerAsync(customerSvc);

        var customer = await customerSvc.GetAsync(customerId);
        Assert.IsNotNull(customer);
        Assert.AreEqual("myCustomer", customer.Name);

        await customerSvc.SetNameAsync(customerId, "myCustomer2");

        customer = await customerSvc.GetAsync(customerId);
        Assert.IsNotNull(customer);
        Assert.AreEqual("myCustomer2", customer.Name);
    }

    [TestMethod]
    public async Task SetContact()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await AddCustomerAsync(customerSvc);

        var customer = await customerSvc.GetAsync(customerId);
        Assert.IsNull(customer!.ContactName);
        Assert.AreEqual("19194122710", customer.ContactPhone?.Value);

        await customerSvc.SetContactAsync(
            customerId,
            new PersonNameDto("Sam", null, "Magura"),
            new PhoneNumberDto("19195551111")
        );
        customer = await customerSvc.GetAsync(customerId);

        Assert.IsNotNull(customer);
        Assert.AreEqual("Sam", customer.ContactName?.First);
        Assert.AreEqual("Magura", customer.ContactName?.Last);
        Assert.AreEqual("19195551111", customer.ContactPhone?.Value);

        await customerSvc.SetContactAsync(customerId, null, null);
        customer = await customerSvc.GetAsync(customerId);
        Assert.IsNull(customer!.ContactName);
        Assert.IsNull(customer.ContactPhone);
    }

    [TestMethod]
    public async Task Remove()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await AddCustomerAsync(customerSvc);
        var customer = await customerSvc.GetAsync(customerId);
        Assert.IsNotNull(customer);

        await customerSvc.RemoveAsync(customerId);
        customer = await customerSvc.GetAsync(customerId);
        Assert.IsNull(customer);
    }

    [TestMethod]
    public async Task AddLtcPharmacy()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await AddCustomerAsync(customerSvc);
        await customerSvc.AddLtcPharmacyAsync(customerId, "1st Choice");

        var customer = await customerSvc.GetAsync(customerId);
        CollectionAssert.AreEquivalent(
            new List<string> { "Pruitt", "Alixa", "1st Choice" },
            customer!.LtcPharmacies.Select(p => p.Name).ToList()
        );
    }

    [TestMethod]
    public async Task RenameLtcPharmacy()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await AddCustomerAsync(customerSvc);
        var customer = await customerSvc.GetAsync(customerId);
        var pruittId = customer!.LtcPharmacies.Single(p => p.Name == "Pruitt").Id;

        await customerSvc.RenameLtcPharmacyAsync(customerId, pruittId, "Pruitt2");

        customer = await customerSvc.GetAsync(customerId);
        CollectionAssert.AreEquivalent(
            new List<string> { "Pruitt2", "Alixa" },
            customer!.LtcPharmacies.Select(p => p.Name).ToList()
        );
    }

    [TestMethod]
    public async Task RemoveLtcPharmacy()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await AddCustomerAsync(customerSvc);
        var customer = await customerSvc.GetAsync(customerId);
        var pruittId = customer!.LtcPharmacies.Single(p => p.Name == "Pruitt").Id;

        await customerSvc.RemoveLtcPharmacyAsync(customerId, pruittId);

        customer = await customerSvc.GetAsync(customerId);
        CollectionAssert.AreEquivalent(
            new List<string> { "Alixa" },
            customer!.LtcPharmacies.Select(p => p.Name).ToList()
        );
    }

    [TestMethod]
    public async Task ItThrowsDuplicateKeyException()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await AddCustomerAsync(customerSvc);

        var e = await Assert.ThrowsExceptionAsync<DuplicateKeyException>(
            () => customerSvc.AddLtcPharmacyAsync(customerId, "Pruitt")
        );

        Assert.AreEqual("Duplicate key: LtcPharmacies value 'Pruitt'", e.Message);
    }
}
