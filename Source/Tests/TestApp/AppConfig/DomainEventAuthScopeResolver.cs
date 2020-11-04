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
        private readonly IOC _ioc;

        public DomainEventAuthScopeResolver(IOC ioc)
        {
            _ioc = ioc;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return _ioc.BeginLifetimeScope(c => {
                c.RegisterType<TestAppSystemAuthContext>().As<IAuthContext>();
            });
        }
    }

}
