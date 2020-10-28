using Autofac;

namespace ITI.DDD.Services.DomainEventsBase
{
    public interface IAuthScopeResolver
    {
        object GetDomainEventHandlerAuthContext();
        ILifetimeScope BeginLifetimeScope(object parentAuthInstance);
    }
}