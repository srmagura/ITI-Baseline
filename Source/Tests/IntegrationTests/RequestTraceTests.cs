using IntegrationTests.Harness;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RequestTrace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.DataContext;
using TestApp.Infrastructure;

namespace IntegrationTests
{
    [TestClass]
    public class RequestTraceTests
    {
        private static TestContext? TestContext;
        private IOC? _ioc;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestContext = context;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _ioc = IntegrationTestInitialize.Initialize(TestContext);
        }

        [TestMethod]
        public void WritesTrace()
        {
            _ioc!.RegisterType<IRequestTrace, DapperRequestTrace>();
            var requestTrace = _ioc.ResolveForTest<IRequestTrace>();

            requestTrace.WriteTrace(
                service: "Google",
                direction: RequestTraceDirection.Incoming,
                dateBeginUtc: DateTimeOffset.UtcNow.AddSeconds(-2),
                url: "myUrl",
                request: "myRequest",
                response: "myResponse",
                exc: new Exception("myException")
            );

            using (var db = new AppDataContext())
            {
                var trace = db.RequestTraces!.Single();

                Assert.AreEqual("Google", trace.Service);
                Assert.AreEqual("Incoming", trace.Direction);
                Assert.AreEqual("myUrl", trace.Url);
                Assert.AreEqual("myRequest", trace.Request);
                Assert.AreEqual("myResponse", trace.Response);
                Assert.IsTrue(trace.Exception!.Contains("myException"));
            }
        }
    }
}
