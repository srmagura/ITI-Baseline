using Autofac;
using ITI.DDD.Application;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Core;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
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
        builder.RegisterModule<ITIDDDModule>();
        builder.RegisterInstance(Substitute.For<IDomainEventPublisher>());

        var logger = Substitute.For<ILogger>();
        builder.RegisterInstance(logger);

        using var container = builder.Build();
        var unitOfWorkProvider = container.Resolve<IUnitOfWorkProvider>();

        Assert.IsNull(unitOfWorkProvider.Current);

        using (var uow = unitOfWorkProvider.Begin())
        {
            Assert.IsNotNull(uow);
            Assert.AreEqual(uow, unitOfWorkProvider.Current);
        }

        Assert.IsNull(unitOfWorkProvider.Current);
        logger.DidNotReceiveWithAnyArgs().Error("");
    }

    private class CustomerAddedEvent : BaseDomainEvent
    {
        public CustomerId CustomerId { get; }

        public CustomerAddedEvent(CustomerId customerId)
        {
            CustomerId = customerId;
        }
    }

    private sealed class DataContext : IDataContext
    {
        public void Dispose()
        {
            // Does not need to be implemented
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
    public async Task CommitPublishesDomainEvents()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<ITIDDDModule>();
        builder.RegisterType<DataContext>();

        var logger = Substitute.For<ILogger>();
        builder.RegisterInstance(logger);

        var domainEventPublisher = Substitute.For<IDomainEventPublisher>();
        builder.RegisterInstance(domainEventPublisher);

        using var container = builder.Build();
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

        logger.DidNotReceiveWithAnyArgs().Error("");
    }
}
