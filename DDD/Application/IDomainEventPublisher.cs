using ITI.DDD.Core;

namespace ITI.DDD.Application;

public interface IDomainEventPublisher
{
    Task PublishAsync(IReadOnlyCollection<IDomainEvent> domainEvents);
}
