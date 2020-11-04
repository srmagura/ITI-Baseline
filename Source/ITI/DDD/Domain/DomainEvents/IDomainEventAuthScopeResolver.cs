using Autofac;

namespace ITI.DDD.Domain.DomainEvents
{
    public interface IDomainEventAuthScopeResolver
    {
        ILifetimeScope BeginLifetimeScope();
    }
}