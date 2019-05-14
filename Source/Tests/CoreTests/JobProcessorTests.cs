using System.Linq;
using AppConfig;
using CoreTests.Helpers;
using DataContext;
using Iti.Core.DateTime;
using Iti.Core.DomainEvents;
using Iti.Email;
using Iti.Inversion;
using Iti.Logging;
using Iti.Logging.Job;
using Iti.Sms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class JobProcessorTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            IOC.RegisterType<IEmailSender, ConsoleEmailSender>();
            IOC.RegisterType<ISmsSender, ConsoleSmsSender>();

            IOC.RegisterType<IEmailRepository, TestEmailRepository>();
            IOC.RegisterType<ISmsRepository, TestSmsRepository>();

            DomainEvents.ClearRegistrations();
        }

        [TestMethod]
        public void LogCleanupTest()
        {
            long logId;

            using (var db = new SampleDataContext())
            {
                var entry = new LogEntry("Test", "test", "test", "test", "test", "test", "test");
                entry.WhenUtc = DateTimeService.UtcNow.AddYears(-1);
                db.LogEntries.Add(entry);
                db.SaveChanges();

                logId = entry.Id;

                var back = db.LogEntries.FirstOrDefault(p => p.Id == logId);
                Assert.IsNotNull(back);
            }

            var proc = IOC.Resolve<LogCleanupJobProcessor>();
            proc.Run();

            using (var db = new SampleDataContext())
            {
                var back = db.LogEntries.FirstOrDefault(p => p.Id == logId);
                Assert.IsNull(back);
            }
        }
    }
}