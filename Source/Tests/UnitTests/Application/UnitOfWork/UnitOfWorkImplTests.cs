using AutoMapper;
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
using TestApp.Domain.Identities;
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
            var domainEvents = Substitute.For<IDomainEvents>();
            var mapper = Substitute.For<IMapper>();
            var auditor = Substitute.For<IAuditor>();
            var uow = new UnitOfWorkImpl(domainEvents, mapper, auditor, new IOC());

            Assert.IsNull(UnitOfWorkImpl.CurrentUnitOfWork);
            uow.Begin();
            Assert.AreEqual(uow, UnitOfWorkImpl.CurrentUnitOfWork);
            uow.Dispose();
            Assert.IsNull(UnitOfWorkImpl.CurrentUnitOfWork);
        }

        public class CustomerAddedEvent : BaseDomainEvent
        {
            public CustomerId CustomerId { get; set; }

            public CustomerAddedEvent(CustomerId customerId)
            {
                CustomerId = customerId;
            }
        }

        private class Customer : Entity
        {
            public CustomerId Id { get; set; } = new CustomerId();

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
            ioc.RegisterInstance(Substitute.For<ILogger>());
            ioc.RegisterInstance(Substitute.For<IAuthContext>());
            ioc.RegisterInstance(Substitute.For<IMapper>());
            ioc.RegisterInstance(Substitute.For<IAuditor>());

            var dataContext = Substitute.For<IDataContext>();
            ioc.RegisterInstance(dataContext);

            dataContext.GetAllDomainEvents().Returns(
                new List<IDomainEvent> 
                { 
                    new CustomerAddedEvent(new CustomerId()) 
                }
            );

            var authScopeResolver = new DomainEventAuthScopeResolverMock(ioc);
            ioc.RegisterInstance<IDomainEventAuthScopeResolver>(authScopeResolver);

            DomainEvents.Register<CustomerAddedEvent, IDomainEventHandler<CustomerAddedEvent>>();
            var eventHandler = Substitute.For<IDomainEventHandler<CustomerAddedEvent>>();
            ioc.RegisterInstance(eventHandler);

            var uow = ioc.Resolve<UnitOfWorkImpl>();

            using (var scope = uow.Begin())
            {
                new Customer();
            }

            // Event discarded since no commit
            eventHandler.DidNotReceive().Handle(Arg.Any<CustomerAddedEvent>());

            using (var scope = uow.Begin())
            {
                var customer =  new Customer();
                scope.Current<IDataContext>();
                scope.Commit();
            }

            eventHandler.Received(1).Handle(Arg.Any<CustomerAddedEvent>());
        }
    }
}
