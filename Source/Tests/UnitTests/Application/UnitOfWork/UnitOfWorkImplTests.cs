using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
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
using UnitTests.Mocks;

namespace UnitTests.Application.UnitOfWork
{
    [TestClass]
    public class UnitOfWorkImplTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DomainEvents.ClearRegistrations();
        }

        [TestMethod]
        public void SetsCurrentUnitOfWork()
        {
            var lifetimeScope = new IOC().BeginLifetimeScope();
            var domainEvents = Substitute.For<IDomainEvents>();
            var uow = new UnitOfWorkImpl(lifetimeScope, domainEvents);

            Assert.IsNull(UnitOfWorkImpl.CurrentUnitOfWork);
            uow.Begin();
            Assert.AreEqual(uow, UnitOfWorkImpl.CurrentUnitOfWork);
            uow.Dispose();
            Assert.IsNull(UnitOfWorkImpl.CurrentUnitOfWork);
        }

        public class CustomerAddedEvent : BaseDomainEvent
        {
            public Guid CustomerId { get; set; }

            public CustomerAddedEvent(Guid customerId)
            {
                CustomerId = customerId;
            }
        }

        private class Customer : Entity
        {
            public Guid Id { get; set; } = Guid.NewGuid();

            public Customer()
            {
                Raise(new CustomerAddedEvent(Id));
            }
        }

        [TestMethod]
        public void CommitHandlesDomainEvents()
        {
            var ioc = new IOC();
            DDDAppConfig.AddRegistrations(ioc);
            var logger = Substitute.For<ILogger>();
            ioc.RegisterInstance(logger);
            var authContext = Substitute.For<IAuthContext>();
            ioc.RegisterInstance(authContext);
            var authScopeResolver = new DomainEventAuthScopeResolverMock(ioc);
            ioc.RegisterInstance<IDomainEventAuthScopeResolver>(authScopeResolver);

            DomainEvents.Register<CustomerAddedEvent, IDomainEventHandler<CustomerAddedEvent>>();
            var eventHandler = Substitute.For<IDomainEventHandler<CustomerAddedEvent>>();
            ioc.RegisterInstance(eventHandler);

            var uow = ioc.ResolveForTest<UnitOfWorkImpl>();

            using (var scope = uow.Begin())
            {
                new Customer();
            }

            // Event discarded since no commit
            eventHandler.DidNotReceive().Handle(Arg.Any<CustomerAddedEvent>());

            Guid customerId;
            using (var scope = uow.Begin())
            {
                var customer =  new Customer();
                customerId = customer.Id;

                scope.Commit();
            }

            eventHandler.Received(1).Handle(
                Arg.Is<CustomerAddedEvent>(e => e.CustomerId == customerId)
            );
        }
    }
}
