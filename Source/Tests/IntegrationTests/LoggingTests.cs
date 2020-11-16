using IntegrationTests.Harness;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Core;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RequestTrace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.AppConfig;
using TestApp.DataContext;
using TestApp.Infrastructure;

namespace IntegrationTests
{
    [TestClass]
    public class LoggingTests
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
        public void WritesLog()
        {
            var logger = _ioc!.ResolveForTest<ILogger>();

            logger.Error("myMessage", new Exception("myException"));

            using (var db = new AppDataContext())
            {
                var logEntry = db.LogEntries!.Single();

                Assert.AreEqual(new TestAppAuthContext().UserId, logEntry.UserId);
                Assert.AreEqual(new TestAppAuthContext().UserName, logEntry.UserName);
                Assert.AreEqual("error", logEntry.Level?.ToLowerInvariant());
                Assert.AreEqual("myMessage", logEntry.Message);
                Assert.IsTrue(logEntry.Exception!.Contains("myException"));
            }
        }
    }
}
