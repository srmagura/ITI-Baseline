using Autofac;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Domain.Entities;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Domain
{
    [TestClass]
    public class DomainEventsTests
    {
        public class CustomerAddedEvent : BaseDomainEvent
        {
            public Guid CustomerId { get; set; }

            public CustomerAddedEvent(Guid customerId)
            {
                CustomerId = customerId;
            }
        }

        private class AuthScopeResolver : IAuthScopeResolver
        {
            private readonly IAuthContext _authContext;
            private readonly IOC _ioc;

            public AuthScopeResolver(IAuthContext authContext, IOC ioc)
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

        [TestMethod]
        public void ProcessSuccess()
        {
            var ioc = new IOC();
            var logger = Substitute.For<ILogger>();
            ioc.RegisterInstance(logger);
            var authContext = Substitute.For<IAuthContext>();
            ioc.RegisterInstance(authContext);
            var authScopeResolver = new AuthScopeResolver(authContext, ioc);
            ioc.RegisterInstance<IAuthScopeResolver>(authScopeResolver);

            DomainEvents.Register<CustomerAddedEvent, IDomainEventHandler<CustomerAddedEvent>>();
            var eventHandler = Substitute.For<IDomainEventHandler<CustomerAddedEvent>>();
            ioc.RegisterInstance(eventHandler);

            var domainEvents = ioc.ResolveForTest<DomainEvents>();
            var ev = new CustomerAddedEvent(Guid.NewGuid());
            domainEvents.Raise(ev);
            domainEvents.HandleAllRaisedEvents();

            eventHandler.Received(1).Handle(Arg.Any<CustomerAddedEvent>());
            eventHandler.Received().Handle(ev);

            logger.DidNotReceive().Error(Arg.Any<string>());
        }
    }
}
