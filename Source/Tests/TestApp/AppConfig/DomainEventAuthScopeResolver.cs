using Autofac;
using ITI.DDD.Auth;
using System;

namespace TestApp.AppConfig
{
    internal class DomainEventAuthScopeResolver //: IDomainEventAuthScopeResolver
    {
        private static ILifetimeScope? RootLifetimeScope;

        public static void OnContainerBuilt(ILifetimeScope rootLifetimeScope)
        {
            RootLifetimeScope = rootLifetimeScope;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            if (RootLifetimeScope == null)
                throw new Exception("OnContainerBuilt must be calld before BeginLifetimeScope.");

            return RootLifetimeScope.BeginLifetimeScope(c =>
            {
                c.RegisterType<TestAppSystemAuthContext>().As<IAuthContext>();
            });
        }
    }
}
