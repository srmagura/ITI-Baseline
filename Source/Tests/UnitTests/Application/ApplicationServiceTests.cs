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
                    () => {
                        if (!allow) throw new NotAuthorizedException();
                    },
                    () => { 
                        if(!entityExists)
                            throw new EntityNotFoundException("test");

                        return new Version("1.0.0");
                    }
                );
            }

            public Version? QueryForNullableObject()
            {
                return Query(
                    () => { },
                    () => {
                        return (Version?) null;
                    }
                );
            }

            public int? QueryForScalar(bool entityExists)
            {
                return QueryScalar(
                    () => { },
                    () => {
                        if (!entityExists)
                            throw new EntityNotFoundException("test");

                        return 1;
                    }
                );
            }

            public int? QueryForNullableScalar()
            {
                return QueryNullableScalar(
                    () => {},
                    () => {
                        return (int?) 1;
                    }
                );
            }

            public void VoidCommand(Action action)
            {
                Command(
                    () => { },
                    () => { action();  }
                );
            }

            public Version? CommandForObject()
            {
                return Command(
                    () => { },
                    () => {
                        return new Version("1.0.0");
                    }
                );
            }

            public Version? CommandForNullableObject()
            {
                return Command(
                    () => { },
                    () => {
                        return (Version?)null;
                    }
                );
            }

            public int? CommandForScalar()
            {
                return CommandScalar(
                    () => { },
                    () => {
                        return 1;
                    }
                );
            }

            public int? CommandForNullableScalar()
            {
                return QueryNullableScalar(
                    () => { },
                    () => {
                        return (int?)1;
                    }
                );
            }
        }

        private MyApplicationService CreateAppService()
        {
            var ioc = new IOC();
            DDDAppConfig.AddRegistrations(ioc);

            ioc.RegisterInstance(Substitute.For<IDomainEventAuthScopeResolver>());
            ioc.RegisterInstance(Substitute.For<ILogger>());
            ioc.RegisterInstance(Substitute.For<IAuthContext>());
            ioc.RegisterInstance(Substitute.For<IMapper>());
            ioc.RegisterInstance(Substitute.For<IAuditor>());

            return ioc.Resolve<MyApplicationService>();
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
        public void QueryForScalar()
        {
            var appService = CreateAppService();

            Assert.AreEqual(1, appService.QueryForScalar(true));
            Assert.IsNull(appService.QueryForScalar(false));
        }

        [TestMethod]
        public void QueryForNullableScalar()
        {
            var appService = CreateAppService();
            Assert.AreEqual(1, appService.QueryForNullableScalar());
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
        public void CommandForScalar()
        {
            var appService = CreateAppService();

            Assert.AreEqual(1, appService.CommandForScalar());
        }

        [TestMethod]
        public void CommandForNullableScalar()
        {
            var appService = CreateAppService();
            Assert.AreEqual(1, appService.CommandForNullableScalar());
        }
    }
}
