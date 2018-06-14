using System;
using System.Linq;
using System.Threading.Tasks;
using AppConfig;
using DataContext;
using Iti.Core.DomainEvents;
using Iti.Logging;
using Iti.Utilities;
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
            Log.DebugEnabled = true;

            var marker = Guid.NewGuid().ToString();

            var task = Task.Run(() =>
            {
                Log.Info(marker);
                Log.Info("Test 1 - Info");
                Log.Debug("Test 1 - Debug");
                Log.Warning("Test 1 - Warning");
                Log.Error("Test 1 - Error");
                Log.Error("Test 2 - Error w/ Exception", new Exception("This is an exception!"));
                Log.Info(marker);
            });

            task.Wait();

            var expectedCount = 7;

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