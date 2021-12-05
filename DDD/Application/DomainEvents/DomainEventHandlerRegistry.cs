namespace ITI.DDD.Application.DomainEvents;

/// <summary>
/// This should be registered using RegisterInstance.
/// </summary>
public class DomainEventHandlerRegistry
{
    public ILookup<Type, Type> Registrations { get; }

    public DomainEventHandlerRegistry(ILookup<Type, Type> registrations)
    {
        Registrations = registrations ?? throw new ArgumentNullException(nameof(registrations));
    }
}
