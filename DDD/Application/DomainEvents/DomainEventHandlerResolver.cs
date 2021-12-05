using Autofac;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Core;

namespace ITI.DDD.Application;

public class DomainEventHandlerResolver : IDomainEventHandlerResolver
{
    private readonly ILifetimeScope _lifetimeScope;
    private readonly ILookup<Type, Type> _registrations;

    public DomainEventHandlerResolver(ILifetimeScope lifetimeScope, DomainEventHandlerRegistry registry)
    {
        _lifetimeScope = lifetimeScope;
        _registrations = registry.Registrations;
    }

    public IReadOnlyCollection<IDomainEventHandler<TEvent>> ResloveHandlers<TEvent>()
        where TEvent : IDomainEvent
    {
        return _registrations[typeof(TEvent)]
            .Select(handlerType => (IDomainEventHandler<TEvent>)_lifetimeScope.Resolve(handlerType))
            .ToList();
    }
}
