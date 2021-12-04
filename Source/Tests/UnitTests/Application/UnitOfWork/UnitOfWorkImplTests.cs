using Autofac;
using Autofac.Features.ResolveAnything;
using AutoMapper;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Domain.Entities;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
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
            var domainEvents = Substitute.For<IDomainEventBuffer>();
            var mapper = Substitute.For<IMapper>();
            var auditor = Substitute.For<IAuditor>();
            var lifetimeScope = new ContainerBuilder().Build().BeginLifetimeScope();
            var uow = new UnitOfWorkImpl(domainEvents, mapper, auditor, lifetimeScope);

            Assert.IsNull(ITI.DDD.Application.UnitOfWork.CurrentUnitOfWork);
            uow.Begin();
            Assert.AreEqual<IUnitOfWork>((IUnitOfWork)uow, ITI.DDD.Application.UnitOfWork.CurrentUnitOfWork);
            uow.Dispose();
            Assert.IsNull(ITI.DDD.Application.UnitOfWork.CurrentUnitOfWork);
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
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            ITIDDDModule.AddRegistrations(builder);
            builder.RegisterInstance(Substitute.For<ILogger>());
            builder.RegisterInstance(Substitute.For<IAuthContext>());
            builder.RegisterInstance(Substitute.For<IMapper>());
            builder.RegisterInstance(Substitute.For<IAuditor>());

            var dataContext = Substitute.For<IDataContext>();
            builder.RegisterInstance(dataContext);

            dataContext.GetAllDomainEvents().Returns(
                new List<IDomainEvent> 
                { 
                    new CustomerAddedEvent(new CustomerId()) 
                }
            );

            builder.Register<IDomainEventAuthScopeResolver>(c =>
                new DomainEventAuthScopeResolverMock(c.Resolve<ILifetimeScope>())
            );

            DomainEvents.Register<CustomerAddedEvent, IDomainEventHandler<CustomerAddedEvent>>();
            var eventHandler = Substitute.For<IDomainEventHandler<CustomerAddedEvent>>();
            builder.RegisterInstance(eventHandler);

            var container = builder.Build();
            var uow = container.Resolve<UnitOfWork>();

            using (var scope = uow.Begin())
            {
                new Customer();
            }

            // Event discarded since no commit
            eventHandler.DidNotReceive().HandleAsync(Arg.Any<CustomerAddedEvent>());

            using (var scope = uow.Begin())
            {
                var customer =  new Customer();
                scope.Current<IDataContext>();
                scope.Commit();
            }

            eventHandler.Received(1).HandleAsync(Arg.Any<CustomerAddedEvent>());
        }
    }
}
