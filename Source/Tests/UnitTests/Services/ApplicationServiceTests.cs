using Iti.Baseline.Inversion;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using ITI.DDD.Services;
using ITI.DDD.Services.UnitOfWorkBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Services
{
    [TestClass]
    public class ApplicationServiceTests
    {
        private class MyApplicationService : ApplicationService
        {
            public MyApplicationService(IUnitOfWork uow, ILogger logger, IAuthContext baseAuth) : base(uow, logger, baseAuth)
            {
            }

            public Version? QueryForObject(bool allow)
            {
                return Query(
                    () => Authorize.Require(allow),
                    () => new Version("1.0.0")
                );
            }

            public int? QueryForScalar()
            {
                return QueryScalar(
                    () => Authorize.AnyUser(),
                    () => 0
                );
            }
        }

        [TestMethod]
        public void Query()
        {
            IOC.RegisterType<ILogger, NullLogWriter>();

            var authContext = Substitute.For<IAuthContext>();
            IOC.RegisterType<IAuthContext, >
            var myAppService = new MyApplicationService();
        }
    }
}
