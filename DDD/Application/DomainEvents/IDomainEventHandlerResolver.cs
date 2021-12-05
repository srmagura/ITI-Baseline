using ITI.DDD.Core;

namespace ITI.DDD.Application;

public interface IDomainEventHandlerResolver
{
    IReadOnlyCollection<IDomainEventHandler<TEvent>> ResloveHandlers<TEvent>()
        where TEvent : IDomainEvent;
}
