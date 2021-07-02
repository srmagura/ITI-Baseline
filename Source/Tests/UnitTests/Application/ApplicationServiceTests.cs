using Autofac;
using AutoMapper;
using ITI.DDD.Application;
using ITI.DDD.Application.Exceptions;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Application
{
    [TestClass]
    public class ApplicationServiceTests
    {
        private class MyApplicationService : ApplicationService
        {
            public MyApplicationService(IUnitOfWork uow, ILogger logger, IAuthContext baseAuth) : base(uow, logger, baseAuth)
            {
            }

            public Task<Version?> QueryForObjectAsync(bool allow, bool entityExists)
            {
                return QueryAsync<Version?>(
                    () =>
                    {
                        if (!allow) throw new NotAuthorizedException();
                        return Task.CompletedTask;
                    },
                    () =>
                    {
                        if (!entityExists)
                            throw new EntityNotFoundException("test");

                        return Task.FromResult<Version?>(new Version("1.0.0"));
                    }
                );
            }

            public Task<Version?> QueryForNullableObjectAsync()
            {
                return QueryAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult((Version?)null)
                );
            }

            public Task<int> QueryForValueAsync(bool entityExists)
            {
                return QueryAsync(
                    () => Task.CompletedTask,
                    () =>
                    {
                        if (!entityExists)
                            throw new EntityNotFoundException("test");

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

            public Task<Version?> CommandForObjectAsync()
            {
                return CommandAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult<Version?>(new Version("1.0.0"))
                );
            }

            public Task<Version?> CommandForNullableObjectAsync()
            {
                return CommandAsync(
                    () => Task.CompletedTask,
                    () => Task.FromResult((Version?)null)
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
        }

        private static MyApplicationService CreateAppService()
        {
            var builder = new ContainerBuilder();
            DDDAppConfig.AddRegistrations(builder);
            builder.RegisterType<MyApplicationService>();

            builder.RegisterInstance(Substitute.For<IDomainEventAuthScopeResolver>());
            builder.RegisterInstance(Substitute.For<ILogger>());
            builder.RegisterInstance(Substitute.For<IAuthContext>());
            builder.RegisterInstance(Substitute.For<IMapper>());
            builder.RegisterInstance(Substitute.For<IAuditor>());

            var container = builder.Build();
            return container.Resolve<MyApplicationService>();
        }

        [TestMethod]
        public async Task QueryForObject()
        {
            var appService = CreateAppService();

            Assert.AreEqual(new Version("1.0.0"), await appService.QueryForObjectAsync(true, true));
            Assert.IsNull(await appService.QueryForObjectAsync(true, false));
            await Assert.ThrowsExceptionAsync<NotAuthorizedException>(
                () => appService.QueryForObjectAsync(false, true)
            );
        }

        [TestMethod]
        public async Task QueryForNullableObject()
        {
            var appService = CreateAppService();
            Assert.IsNull(await appService.QueryForNullableObjectAsync());
        }

        [TestMethod]
        public async Task QueryForValue()
        {
            var appService = CreateAppService();

            Assert.AreEqual(1, await appService.QueryForValueAsync(true));
            Assert.AreEqual(0, await appService.QueryForValueAsync(false));
        }

        [TestMethod]
        public async Task QueryForNullableValue()
        {
            var appService = CreateAppService();
            Assert.AreEqual(1, await appService.QueryForNullableValueAsync());
        }

        [TestMethod]
        public async Task VoidCommand()
        {
            var appService = CreateAppService();
            var action = Substitute.For<Func<Task>>();
            await appService.VoidCommandAsync(action);

            await action.Received().Invoke();
        }

        [TestMethod]
        public async Task CommandForObject()
        {
            var appService = CreateAppService();
            Assert.AreEqual(new Version("1.0.0"), await appService.CommandForObjectAsync());
        }

        [TestMethod]
        public async Task CommandForNullableObject()
        {
            var appService = CreateAppService();
            Assert.IsNull(await appService.CommandForNullableObjectAsync());
        }

        [TestMethod]
        public async Task CommandForValue()
        {
            var appService = CreateAppService();

            Assert.AreEqual(1, await appService.CommandForValueAsync());
        }

        [TestMethod]
        public async Task CommandForNullableValue()
        {
            var appService = CreateAppService();
            Assert.AreEqual(1, await appService.CommandForNullableValueAsync());
        }
    }
}
