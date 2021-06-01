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

            public Version? QueryForObject(bool allow, bool entityExists)
            {
                return Query(
                    () =>
                    {
                        if (!allow) throw new NotAuthorizedException();
                    },
                    () =>
                    {
                        if (!entityExists)
                            throw new EntityNotFoundException("test");

                        return new Version("1.0.0");
                    }
                );
            }

            public Version? QueryForNullableObject()
            {
                return Query(
                    () => { },
                    () =>
                    {
                        return (Version?)null;
                    }
                );
            }

            public int? QueryForValue(bool entityExists)
            {
                return QueryValue(
                    () => { },
                    () =>
                    {
                        if (!entityExists)
                            throw new EntityNotFoundException("test");

                        return 1;
                    }
                );
            }

            public int? QueryForNullableValue()
            {
                return QueryNullableValue(
                    () => { },
                    () =>
                    {
                        return (int?)1;
                    }
                );
            }

            public void VoidCommand(Action action)
            {
                Command(
                    () => { },
                    () => { action(); }
                );
            }

            public Version? CommandForObject()
            {
                return Command(
                    () => { },
                    () =>
                    {
                        return new Version("1.0.0");
                    }
                );
            }

            public Version? CommandForNullableObject()
            {
                return Command(
                    () => { },
                    () =>
                    {
                        return (Version?)null;
                    }
                );
            }

            public int? CommandForValue()
            {
                return CommandValue(
                    () => { },
                    () =>
                    {
                        return 1;
                    }
                );
            }

            public int? CommandForNullableValue()
            {
                return QueryNullableValue(
                    () => { },
                    () =>
                    {
                        return (int?)1;
                    }
                );
            }
        }

        private MyApplicationService CreateAppService()
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
        public void QueryForObject()
        {
            var appService = CreateAppService();

            Assert.AreEqual(new Version("1.0.0"), appService.QueryForObject(true, true));
            Assert.IsNull(appService.QueryForObject(true, false));
            Assert.ThrowsException<NotAuthorizedException>(
                () => appService.QueryForObject(false, true)
            );
        }

        [TestMethod]
        public void QueryForNullableObject()
        {
            var appService = CreateAppService();
            Assert.IsNull(appService.QueryForNullableObject());
        }

        [TestMethod]
        public void QueryForValue()
        {
            var appService = CreateAppService();

            Assert.AreEqual(1, appService.QueryForValue(true));
            Assert.AreEqual(0, appService.QueryForValue(false));    // nullable reference types disabled currently
        }

        [TestMethod]
        public void QueryForNullableValue()
        {
            var appService = CreateAppService();
            Assert.AreEqual(1, appService.QueryForNullableValue());
        }

        [TestMethod]
        public void VoidCommand()
        {
            var appService = CreateAppService();
            var action = Substitute.For<Action>();
            appService.VoidCommand(action);

            action.Received().Invoke();
        }

        [TestMethod]
        public void CommandForObject()
        {
            var appService = CreateAppService();
            Assert.AreEqual(new Version("1.0.0"), appService.CommandForObject());
        }

        [TestMethod]
        public void CommandForNullableObject()
        {
            var appService = CreateAppService();
            Assert.IsNull(appService.CommandForNullableObject());
        }

        [TestMethod]
        public void CommandForValue()
        {
            var appService = CreateAppService();

            Assert.AreEqual(1, appService.CommandForValue());
        }

        [TestMethod]
        public void CommandForNullableValue()
        {
            var appService = CreateAppService();
            Assert.AreEqual(1, appService.CommandForNullableValue());
        }
    }
}
