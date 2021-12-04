using TestApp.Domain.Identities;

namespace TestApp.Domain.Events;

public class CustomerAddedEvent : BaseDomainEvent
{
    public CustomerId CustomerId { get; }
    public string Name { get; }

    public CustomerAddedEvent(CustomerId customerId, string name)
    {
        CustomerId = customerId;
        Name = name;
    }
}
