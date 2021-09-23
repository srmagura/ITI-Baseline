using ITI.DDD.Domain.DomainEvents;
using TestApp.Domain.Identities;

namespace TestApp.Domain.Events
{
    public class CustomerAddressChangedEvent : BaseDomainEvent
    {
        public CustomerId Id { get; set; }

        public CustomerAddressChangedEvent(CustomerId id)
        {
            Id = id;
        }
    }
}
