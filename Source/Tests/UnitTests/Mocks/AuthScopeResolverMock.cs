using Autofac;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Mocks
{
    internal class AuthScopeResolverMock : IAuthScopeResolver
    {
        private readonly IAuthContext _authContext;
        private readonly IOC _ioc;

        public AuthScopeResolverMock(IAuthContext authContext, IOC ioc)
        {
            _authContext = authContext;
            _ioc = ioc;
        }

        public ILifetimeScope BeginLifetimeScope(object parentAuthInstance)
        {
            return _ioc.BeginLifetimeScope();
        }

        public object GetDomainEventHandlerAuthContext()
        {
            return _authContext;
        }
    }

}
