using Autofac;
using Autofac.Features.ResolveAnything;
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
using System.Threading;
using System.Threading.Tasks;
using TestApp.Domain.Identities;
using UnitTests.Mocks;

namespace UnitTests.Domain
{
    [TestClass]
    public class DomainEventsTests
    {
        public class CustomerAddedEvent : BaseDomainEvent
        {
            public CustomerId CustomerId { get; set; }

            public CustomerAddedEvent(CustomerId customerId)
            {
                CustomerId = customerId;
            }
        }

        public class VendorAddedEvent : BaseDomainEvent
        {
            public VendorId VendorId { get; set; }

            public VendorAddedEvent(VendorId vendorId)
            {
                VendorId = vendorId;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            DomainEvents.ClearRegistrations();
        }

        private ContainerBuilder GetContainerBuilder(out ILogger logger)
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            logger = Substitute.For<ILogger>();
            builder.RegisterInstance(logger);

            var authContext = Substitute.For<IAuthContext>();
            builder.RegisterInstance(authContext);

            builder.Register<IDomainEventAuthScopeResolver>(c =>
                new DomainEventAuthScopeResolverMock(c.Resolve<ILifetimeScope>())
            );

            return builder;
        }

        [TestMethod]
        public async Task ProcessSuccess()
        {
            var builder = GetContainerBuilder(out var logger);

            DomainEvents.Register<CustomerAddedEvent, IDomainEventHandler<CustomerAddedEvent>>();
            var eventHandler = Substitute.For<IDomainEventHandler<CustomerAddedEvent>>();
            builder.RegisterInstance(eventHandler);

            var container = builder.Build();
            var domainEvents = container.Resolve<DomainEvents>();
            
            var ev = new CustomerAddedEvent(new CustomerId());
            domainEvents.Raise(ev);
            await domainEvents.HandleAllRaisedEventsAsync();

            await eventHandler.Received(1).HandleAsync(Arg.Any<CustomerAddedEvent>());
            await eventHandler.Received().HandleAsync(ev);

            logger.DidNotReceive().Error(Arg.Any<string>());
        }

        [TestMethod]
        public async Task ProcessFailure()
        {
            var builder = GetContainerBuilder(out var logger);

            DomainEvents.Register<CustomerAddedEvent, IDomainEventHandler<CustomerAddedEvent>>();
            var eventHandler = Substitute.For<IDomainEventHandler<CustomerAddedEvent>>();
            eventHandler
                .When(x => x.HandleAsync(Arg.Any<CustomerAddedEvent>()))
                .Do(x => { throw new Exception("myException"); });
            builder.RegisterInstance(eventHandler);

            var container = builder.Build();
            var domainEvents = container.Resolve<DomainEvents>();

            var ev = new CustomerAddedEvent(new CustomerId());
            domainEvents.Raise(ev);
            await domainEvents.HandleAllRaisedEventsAsync();

            await eventHandler.Received(1).HandleAsync(Arg.Any<CustomerAddedEvent>());
            await eventHandler.Received().HandleAsync(ev);

            logger.Received().Error(
                Arg.Any<string>(),
                Arg.Is<Exception>(e => e.InnerException != null && e.InnerException.Message == "myException")
            );
        }

        [TestMethod]
        public async Task NoHandlerRegistered()
        {
            var builder = GetContainerBuilder(out var logger);
            var container = builder.Build();
            var domainEvents = container.Resolve<DomainEvents>();

            var ev = new CustomerAddedEvent(new CustomerId());
            domainEvents.Raise(ev);
            await domainEvents.HandleAllRaisedEventsAsync();

            logger.DidNotReceive().Error(Arg.Any<string>());
        }

        [TestMethod]
        public async Task FirstHandlerFailsSecondHandlerSucceeds()
        {
            var builder = GetContainerBuilder(out var logger);

            // CustomerAddedEvent handler (fails)
            DomainEvents.Register<CustomerAddedEvent, IDomainEventHandler<CustomerAddedEvent>>();
            var customerEventHandler = Substitute.For<IDomainEventHandler<CustomerAddedEvent>>();
            customerEventHandler
                .When(x => x.HandleAsync(Arg.Any<CustomerAddedEvent>()))
                .Do(x => { throw new Exception("myException"); });
            builder.RegisterInstance(customerEventHandler);

            // VendorAddedEvent handler (succeeds)
            DomainEvents.Register<VendorAddedEvent, IDomainEventHandler<VendorAddedEvent>>();
            var vendorEventHandler = Substitute.For<IDomainEventHandler<VendorAddedEvent>>();
            builder.RegisterInstance(vendorEventHandler);

            var container = builder.Build();
            var domainEvents = container.Resolve<DomainEvents>();
            
            // Raise events
            var customerEvent = new CustomerAddedEvent(new CustomerId());
            var vendorEvent = new VendorAddedEvent(new VendorId());
            domainEvents.Raise(customerEvent);
            domainEvents.Raise(vendorEvent);
            await domainEvents.HandleAllRaisedEventsAsync();

            await customerEventHandler.Received(1).HandleAsync(customerEvent);
            await vendorEventHandler.Received(1).HandleAsync(vendorEvent);

            logger.Received().Error(
                Arg.Any<string>(),
                Arg.Is<Exception>(e => e.InnerException != null && e.InnerException.Message == "myException")
            );
        }
    }
}
