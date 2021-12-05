using TestApp.Domain.Identities;

namespace TestApp.Domain.Events;

public class CustomerAddressChangedEvent : BaseDomainEvent
{
    public CustomerId Id { get; }

    public CustomerAddressChangedEvent(CustomerId id)
    {
        Id = id;
    }
}
