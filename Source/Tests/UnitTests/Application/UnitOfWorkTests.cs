using Autofac;
using Autofac.Features.ResolveAnything;
using AutoMapper;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestApp.Domain.Events;
using TestApp.Domain.Identities;

namespace UnitTests.Application;

[TestClass]
public class UnitOfWorkTests
{
    [TestMethod]
    public void ItSetsCurrentUnitOfWork()
    {
        var builder = new ContainerBuilder();
        builder.RegisterInstance(Substitute.For<IDomainEventPublisher>());

        using var container = builder.Build();
        using var lifetimeScope = container.BeginLifetimeScope();

        var unitOfWorkProvider = new UnitOfWorkProvider(lifetimeScope);

        Assert.IsNull(unitOfWorkProvider.Current);

        using (var uow = unitOfWorkProvider.Begin())
        {
            Assert.IsNotNull(uow);
            Assert.AreEqual(uow, unitOfWorkProvider.Current);
        }

        Assert.IsNull(unitOfWorkProvider.Current);
    }

    private class CustomerAddedEvent : BaseDomainEvent
    {
        public CustomerId CustomerId { get; }

        public CustomerAddedEvent(CustomerId customerId)
        {
            CustomerId = customerId;
        }
    }

    private class DataContext : IDataContext
    {
        public void Dispose()
        {
        }

        public List<IDomainEvent> GetAllDomainEvents()
        {
            return new List<IDomainEvent>
            {
                new CustomerAddedEvent(new CustomerId())
            };
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }

    [TestMethod]
    public async Task CommitHandlesDomainEvents()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<ITIDDDModule>();
        builder.RegisterType<DataContext>();

        var domainEventPublisher = Substitute.For<IDomainEventPublisher>();
        builder.RegisterInstance(domainEventPublisher);

        var container = builder.Build();
        var unitOfWorkProvider = container.Resolve<IUnitOfWorkProvider>();

        using (var uow = unitOfWorkProvider.Begin())
        {
            uow.GetDataContext<DataContext>();
        }

        // Event discarded since no commit
        await domainEventPublisher.DidNotReceiveWithAnyArgs().PublishAsync(new List<IDomainEvent>());

        using (var uow = unitOfWorkProvider.Begin())
        {
            uow.GetDataContext<DataContext>();
            await uow.CommitAsync();
        }

        await domainEventPublisher.Received(1).PublishAsync(
            Arg.Is<IReadOnlyCollection<IDomainEvent>>(c => c.Count == 1)
        );
    }
}
