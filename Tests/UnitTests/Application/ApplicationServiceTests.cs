using Autofac;
using ITI.DDD.Application;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using TestApp.Domain.Events;
using TestApp.Domain.Identities;

namespace UnitTests.Application
{
    [TestClass]
    public class ApplicationServiceTests
    {
        private class MyApplicationService : ApplicationService
        {
            public MyApplicationService(IUnitOfWorkProvider uow, ILogger logger, IAuthContext baseAuth) : base(uow, logger, baseAuth)
            {
            }

            public Task<Version> QueryForObjectAsync()
            {
                return QueryAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult(new Version("1.0.0"))
                );
            }

            public Task<Version?> QueryForNullableObjectAsync(bool allow, bool entityExists)
            {
                return QueryAsync(
                    () =>
                    {
                        if (!allow) throw new NotAuthorizedException();
                        return Task.CompletedTask;
                    },
                    () =>
                    {
                        if (!entityExists) return Task.FromResult<Version?>(null);

                        return Task.FromResult<Version?>(new Version("1.0.0"));
                    }
                );
            }

            public Task<int> QueryForValueAsync(bool entityExists)
            {
                return QueryAsync(
                    () => Task.CompletedTask,
                    () =>
                    {
                        if (!entityExists) return Task.FromResult(0);

                        return Task.FromResult(1);
                    }
                );
            }

            public Task<int?> QueryForNullableValueAsync()
            {
                return QueryAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult((int?)1)
                );
            }

            public Task VoidCommandAsync(Func<Task> action)
            {
                return CommandAsync(
                    () => Task.CompletedTask,
                    async () =>
                    {
                        await action();
                    }
                );
            }

            public Task<Version> CommandForObjectAsync()
            {
                return CommandAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult(new Version("1.0.0"))
                );
            }

            public Task<Version?> CommandForNullableObjectAsync()
            {
                return CommandAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult<Version?>(new Version("1.0.0"))
                );
            }

            public Task<int> CommandForValueAsync()
            {
                return CommandAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult(1)
                );
            }

            public Task<int?> CommandForNullableValueAsync()
            {
                return CommandAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult((int?)1)
                );
            }

            public Task RaiseADomainEventAsync()
            {
                return CommandAsync(
                    () => Task.CompletedTask,
                    () =>
                    {
                        RaiseDomainEvent(new CustomerAddedEvent(new CustomerId(), "Foobar Customer"));
                        return Task.CompletedTask;
                    }
                );
            }
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<ITIDDDModule>();
            builder.RegisterType<MyApplicationService>();

            builder.RegisterInstance(Substitute.For<ILogger>());
            builder.RegisterInstance(Substitute.For<IAuthContext>());

            var domainEventPublisher = Substitute.For<IDomainEventPublisher>();
            builder.RegisterInstance(domainEventPublisher);

            return builder.Build();
        }

        [TestMethod]
        public async Task QueryForNullableObject()
        {
            using var container = BuildContainer();
            var appService = container.Resolve<MyApplicationService>();

            Assert.AreEqual(new Version("1.0.0"), await appService.QueryForNullableObjectAsync(allow: true, entityExists: true));
            Assert.IsNull(await appService.QueryForNullableObjectAsync(allow: true, entityExists: false));
            await Assert.ThrowsExceptionAsync<NotAuthorizedException>(
                () => appService.QueryForNullableObjectAsync(allow: false, entityExists: true)
            );
        }

        [TestMethod]
        public async Task QueryForValue()
        {
            using var container = BuildContainer();
            var appService = container.Resolve<MyApplicationService>();

            Assert.AreEqual(1, await appService.QueryForValueAsync(true));
            Assert.AreEqual(0, await appService.QueryForValueAsync(false));
        }

        [TestMethod]
        public async Task QueryForNullableValue()
        {
            using var container = BuildContainer();
            var appService = container.Resolve<MyApplicationService>();
            Assert.AreEqual(1, await appService.QueryForNullableValueAsync());
        }

        [TestMethod]
        public async Task VoidCommand()
        {
            using var container = BuildContainer();
            var appService = container.Resolve<MyApplicationService>();
            var action = Substitute.For<Func<Task>>();
            await appService.VoidCommandAsync(action);

            await action.Received().Invoke();
        }

        [TestMethod]
        public async Task CommandForNullableObject()
        {
            using var container = BuildContainer();
            var appService = container.Resolve<MyApplicationService>();
            Assert.AreEqual(new Version("1.0.0"), await appService.CommandForNullableObjectAsync());
        }

        [TestMethod]
        public async Task CommandForValue()
        {
            using var container = BuildContainer();
            var appService = container.Resolve<MyApplicationService>();
            Assert.AreEqual(1, await appService.CommandForValueAsync());
        }

        [TestMethod]
        public async Task CommandForNullableValue()
        {
            using var container = BuildContainer();
            var appService = container.Resolve<MyApplicationService>();
            Assert.AreEqual(1, await appService.CommandForNullableValueAsync());
        }

        [TestMethod]
        public async Task RaiseADomainEvent()
        {
            using var container = BuildContainer();
            var appService = container.Resolve<MyApplicationService>();
            var domainEventPublisher = container.Resolve<IDomainEventPublisher>();

            await appService.RaiseADomainEventAsync();

            await domainEventPublisher.Received(1).PublishAsync(
                Arg.Is<IReadOnlyCollection<IDomainEvent>>(c => c.Count == 1)
            );
        }
    }
}
