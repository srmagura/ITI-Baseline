using ITI.DDD.Core;

namespace ITI.DDD.Application;

public interface IDomainEventHandler<in T>
    where T : IDomainEvent
{
    Task HandleAsync(T domainEvent);
}
