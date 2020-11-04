using Autofac;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Mocks
{
    internal class DomainEventAuthScopeResolverMock : IDomainEventAuthScopeResolver
    {
        private readonly IOC _ioc;

        public DomainEventAuthScopeResolverMock(IOC ioc)
        {
            _ioc = ioc;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return _ioc.BeginLifetimeScope();
        }
    }

}
