using Autofac;
using ITI.DDD.Application.DomainEvents.Direct;
using ITI.DDD.Auth;

namespace TestApp.AppConfig;

internal class DirectDomainEventPublisherLifetimeScopeProvider : IDirectDomainEventPublisherLifetimeScopeProvider
{
    private static ILifetimeScope? _rootLifetimeScope;

    public static void OnContainerBuilt(ILifetimeScope rootLifetimeScope)
    {
        _rootLifetimeScope = rootLifetimeScope;
    }

    public ILifetimeScope BeginLifetimeScope()
    {
        if (_rootLifetimeScope == null)
        {
            throw new InvalidOperationException(
                "OnContainerBuilt must be calld before BeginLifetimeScope."
            );
        }

        return _rootLifetimeScope.BeginLifetimeScope(c =>
        {
            c.RegisterType<SystemAuthContext>().As<IAuthContext>();
        });
    }
}
