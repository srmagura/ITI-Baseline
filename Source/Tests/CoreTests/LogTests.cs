using System;
using System.Linq;
using System.Threading.Tasks;
using AppConfig;
using CoreTests.Helpers;
using DataContext;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Logging;
using Iti.Baseline.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class LogTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();

            DomainEvents.ClearRegistrations();
        }

        [TestMethod]
        public void BasicLogTests()
        {
            var Log = new Logger(new DbLogWriter(new DbLoggerSettings() { ConnectionString = SampleDataContext.GetConnectionString() }), new TestAuthContext());

            var marker = Guid.NewGuid().ToString();

            var task = Task.Run(() =>
            {
                Log.Info(marker);
                Log.Info("Test 1 - Info");
                Log.Warning("Test 1 - Warning");
                Log.Error("Test 1 - Error");
                Log.Error("Test 2 - Error w/ Exception", new Exception("This is an exception!"));
                Log.Info(marker);
            });

            task.Wait();

            var expectedCount = 6;

            using (var db = new SampleDataContext())
            {
                var logs = db.LogEntries
                    .OrderByDescending(p => p.Id)
                    .Take(expectedCount)
                    .OrderBy(p => p.Id)
                    .ToList();

                logs.ConsoleDump();

                Assert.AreEqual(expectedCount, logs.Count);

                Assert.AreEqual(marker, logs[0].Message);
                Assert.AreEqual(marker, logs[expectedCount - 1].Message);
            }
        }
    }
}