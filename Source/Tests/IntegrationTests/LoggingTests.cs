using IntegrationTests.Harness;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using TestApp.AppConfig;
using TestApp.DataContext;
using Autofac;

namespace IntegrationTests
{
    [TestClass]
    public class LoggingTests : IntegrationTest
    {
        [TestMethod]
        public void WritesLog()
        {
            var builder = new ContainerBuilder();
            RegisterServices(builder);
            builder.RegisterType<DbLogWriter>().As<ILogWriter>();
            Container = builder.Build();

            var logger = Container.Resolve<ILogger>();

            logger.Error("myMessage", new Exception("myException"));

            using var db = Container.Resolve<AppDataContext>();
            var logEntry = db.LogEntries!.Single();

            Assert.AreEqual(new AppAuthContext().UserIdString, logEntry.UserId);
            Assert.AreEqual(new AppAuthContext().UserName, logEntry.UserName);
            Assert.AreEqual("error", logEntry.Level?.ToLowerInvariant());
            Assert.AreEqual("myMessage", logEntry.Message);
            Assert.IsTrue(logEntry.Exception!.Contains("myException"));
        }
    }
}
