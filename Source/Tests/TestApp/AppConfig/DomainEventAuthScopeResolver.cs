using Autofac;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.AppConfig;

namespace UnitTests.Mocks
{
    internal class DomainEventAuthScopeResolver : IDomainEventAuthScopeResolver
    {
        private static ILifetimeScope RootLifetimeScope;

        public static void OnContainerBuilt(ILifetimeScope rootLifetimeScope)
        {
            RootLifetimeScope = rootLifetimeScope;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return RootLifetimeScope.BeginLifetimeScope(c => {
                c.RegisterType<TestAppSystemAuthContext>().As<IAuthContext>();
            });
        }
    }
}
