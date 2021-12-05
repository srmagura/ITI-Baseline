using ITI.DDD.Core;

namespace ITI.DDD.Application.DomainEvents;

public class DomainEventHandlerRegistryBuilder
{
    private readonly List<(Type EventType, Type HandlerType)> _registrations = new();

    private bool _isBuilt = false;

    public void Register<TEvent, THandler>()
        where TEvent : IDomainEvent
        where THandler : IDomainEventHandler<TEvent>
    {
        if (_isBuilt)
            throw new InvalidOperationException("You cannot register after the registry has been built.");

        _registrations.Add((typeof(TEvent), typeof(THandler)));
    }

    public DomainEventHandlerRegistry Build()
    {
        if (_isBuilt)
            throw new InvalidOperationException("This registry builder has already been built.");

        _isBuilt = true;

        var lookup = _registrations.ToLookup(t => t.EventType, t => t.HandlerType);
        return new DomainEventHandlerRegistry(lookup);
    }
}
