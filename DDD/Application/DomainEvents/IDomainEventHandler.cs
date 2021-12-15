using ITI.DDD.Core;

namespace ITI.DDD.Application.DomainEvents;

public interface IDomainEventHandler<in T>
    where T : IDomainEvent
{
    Task HandleAsync(T domainEvent, CancellationToken cancellationToken = default);
}
