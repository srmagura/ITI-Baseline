using ITI.DDD.Application.DomainEvents;
using TestApp.Domain.Events;

namespace TestApp.Application.EventHandlers;

public class CustomerEventHandler : IDomainEventHandler<CustomerAddedEvent>
{
    public static void Register(DomainEventHandlerRegistryBuilder builder)
    {
        builder.Register<CustomerAddedEvent, CustomerEventHandler>();
    }

    public static CustomerAddedEvent? LastHandledEvent { get; private set; }

    public Task HandleAsync(CustomerAddedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        LastHandledEvent = domainEvent;
        return Task.CompletedTask;
    }
}
