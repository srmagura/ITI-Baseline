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
        private readonly ILifetimeScope _scope;

        public DomainEventAuthScopeResolverMock(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return _scope.BeginLifetimeScope();
        }
    }

}
