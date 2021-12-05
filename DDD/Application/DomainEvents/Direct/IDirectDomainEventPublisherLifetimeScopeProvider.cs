using Autofac;

namespace ITI.DDD.Application.DomainEvents.Direct;

public interface IDirectDomainEventPublisherLifetimeScopeProvider
{
    ILifetimeScope BeginLifetimeScope();
}
