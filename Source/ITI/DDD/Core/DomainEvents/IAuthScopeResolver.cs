using Autofac;

namespace ITI.DDD.Domain.DomainEvents
{
    public interface IAuthScopeResolver
    {
        object GetDomainEventHandlerAuthContext();
        ILifetimeScope BeginLifetimeScope(object parentAuthInstance);
    }
}