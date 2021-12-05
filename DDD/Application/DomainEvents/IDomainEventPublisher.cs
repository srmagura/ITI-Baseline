using ITI.DDD.Core;

namespace ITI.DDD.Application.DomainEvents;

public interface IDomainEventPublisher
{
    Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents);
}
