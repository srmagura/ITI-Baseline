using Autofac;
using IntegrationTests.Harness;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.Application.EventHandlers;
using TestApp.Application.Interfaces;

namespace IntegrationTests;

[TestClass]
public class DomainEventTests : IntegrationTest
{
    [TestMethod]
    public async Task ItHandlesADomainEvent()
    {
        var customerSvc = Container.Resolve<ICustomerAppService>();

        var customerId = await customerSvc.AddAsync(
            name: "myCustomer",
            address: null,
            contactName: null,
            contactPhone: null
        );

        var domainEvent = CustomerEventHandler.LastHandledEvent;
        Assert.IsNotNull(domainEvent);
        Assert.AreEqual(customerId, domainEvent.CustomerId);
    }
}
