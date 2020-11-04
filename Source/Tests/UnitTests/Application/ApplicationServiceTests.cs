using Autofac;
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
        }

        [TestMethod]
        public void QueryForObject()
        {
            var ioc = new IOC();
            DDDAppConfig.AddRegistrations(ioc);

            ioc.RegisterInstance(Substitute.For<IDomainEventAuthScopeResolver>());
            ioc.RegisterInstance(Substitute.For<ILogger>());
            ioc.RegisterInstance(Substitute.For<IAuthContext>());

            var appService = ioc.ResolveForTest<MyApplicationService>();

            Assert.AreEqual(new Version("1.0.0"), appService.QueryForObject(true, true));
            Assert.IsNull(appService.QueryForObject(true, false));
            Assert.ThrowsException<NotAuthorizedException>(
                () => appService.QueryForObject(false, true)
            );
        }

        [TestMethod]
        public void QueryForScalar()
        {
            var ioc = new IOC();
            DDDAppConfig.AddRegistrations(ioc);

            ioc.RegisterInstance(Substitute.For<IDomainEventAuthScopeResolver>());
            ioc.RegisterInstance(Substitute.For<ILogger>());
            ioc.RegisterInstance(Substitute.For<IAuthContext>());

            var appService = ioc.ResolveForTest<MyApplicationService>();

            Assert.AreEqual(1, appService.QueryForScalar(true));
            Assert.IsNull(appService.QueryForScalar(false));
        }
    }
}
