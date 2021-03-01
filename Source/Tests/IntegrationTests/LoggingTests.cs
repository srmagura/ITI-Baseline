using IntegrationTests.Harness;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Core;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITI.Baseline.RequestTrace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.AppConfig;
using TestApp.DataContext;
using TestApp.Infrastructure;
using Autofac;

namespace IntegrationTests
{
    [TestClass]
    public class LoggingTests
    {
        private static TestContext? TestContext;
        private IContainer? _container;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestContext = context;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container = IntegrationTestInitialize.Initialize(TestContext).Build();
        }

        [TestMethod]
        public void WritesLog()
        {
            var logger = _container!.Resolve<ILogger>();

            logger.Error("myMessage", new Exception("myException"));

            using (var db = _container.Resolve<AppDataContext>())
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
