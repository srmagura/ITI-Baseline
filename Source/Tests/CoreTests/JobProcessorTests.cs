using System.Linq;
using AppConfig;
using Autofac;
using CoreTests.Helpers;
using DataContext;
using Iti.Baseline.Auth;
using Iti.Baseline.Core.DateTime;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Email;
using Iti.Baseline.Inversion;
using Iti.Baseline.Logging;
using Iti.Baseline.Logging.Job;
using Iti.Baseline.Sms;
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

            IOC.RegisterType<IAuthContext, TestAuthContext>();

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

            var proc = IOC.ResolveForTest<LogCleanupJobProcessor>();
            proc.Run();

            using (var db = new SampleDataContext())
            {
                var back = db.LogEntries.FirstOrDefault(p => p.Id == logId);
                Assert.IsNull(back);
            }
        }
    }
}