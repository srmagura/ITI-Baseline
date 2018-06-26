using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using AppConfig;
using AutoMapper;
using CoreTests.Helpers;
using DataContext;
using Iti.Auth;
using Iti.Core.DomainEvents;
using Iti.Core.Entites;
using Iti.Inversion;
using Iti.Logging;
using Iti.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApp.Auth;

namespace CoreTests
{
    [TestClass]
    public class AuditTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            DomainEvents.ClearRegistrations();

            IOC.RegisterType<IAuthContext, TestAuthContext>();
            IOC.RegisterType<IAppAuthContext, TestAuthContext>();

            IOC.RegisterType<ILogWriter, ConsoleLogWriter>();
        }

        [TestMethod]
        public void ValueObjectTest()
        {
            long auditId = 0;

            var fooId = SequentialGuid.Next();

            var nullAddr = Activator.CreateInstance(typeof(Address),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new object[] { }, null) as Address;
            var addr1 = new Address("1", "1", "1", "1", "1");
            var addr2 = new Address("2", "2", "2", "2", "2");

            Section("CREATE");

            using (var db = new SampleDataContext())
            {
                var foo = new DbFoo()
                {
                    Id = fooId,
                    Name = "Created",
                    NotInEntity = "Foo!",
                    Address = nullAddr,
                };

                db.Foos.Add(foo);
                db.SaveChanges();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("SET ADDRESS 1");

            using (var db = new SampleDataContext())
            {
                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                Assert.IsNull(foo.Address.City);

                // foo.Name = "Set Address 1";

                // foo.Address = addr1;
                Mapper.Map(addr1, foo.Address);

                db.SaveChanges();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("SET ADDRESS 2");

            using (var db = new SampleDataContext())
            {
                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                Assert.AreEqual("1", foo.Address.City);

                // foo.Name = "Set Address 2";

                // foo.Address = addr2;
                Mapper.Map(addr2, foo.Address);

                db.SaveChanges();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("Read Back");

            using (var db = new SampleDataContext())
            {
                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                Assert.AreEqual("2", foo.Address.City);
            }

            Section("Add Bar");

            using (var db = new SampleDataContext())
            {
                var foo = db.Foos
                    .Include(p => p.Bars)
                    .FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                foo.Bars.Add(new DbBar
                {
                    Id = SequentialGuid.Next(),
                    Name = Guid.NewGuid().ToString(),
                    NotInEntity = "RemoveMe!",
                });

                foo.Bars.Add(new DbBar
                {
                    Id = SequentialGuid.Next(),
                    Name = Guid.NewGuid().ToString(),
                    NotInEntity = "DoNotRemoveMe",
                });

                db.SaveChanges();

                AssertBarCount(fooId, 2);

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("Remove Bar");

            using (var db = new SampleDataContext())
            {
                var foo = db.Foos
                    .Include(p => p.Bars)
                    .FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                foo.Bars.RemoveAll(p => p.NotInEntity == "RemoveMe!");

                db.SaveChanges();

                AssertBarCount(fooId, 1);

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("Remove");

            using (var db = new SampleDataContext())
            {
                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                db.Foos.Remove(foo);
                db.SaveChanges();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }
        }

        private void AssertBarCount(Guid fooId, int expectedCount)
        {
            using (var db = new SampleDataContext())
            {
                var barCount = db.Bars.Count(p => p.FooId == fooId);
                Assert.AreEqual(expectedCount, barCount);
            }
        }

        private void Section(string tag)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {tag} ===================================================================");
            Console.WriteLine();
        }

        private long DumpAudit(Guid fooId, long lastId)
        {
            using (var db = new SampleDataContext())
            {
                var auditList = db.AuditEntries
                    .Where(p => p.Aggregate == "Foo" && p.AggregateId == fooId.ToString())
                    .Where(p => p.Id > lastId)
                    .OrderByDescending(p => p.Id)
                    .ToList();

                Assert.IsNotNull(auditList);
                Assert.IsTrue(auditList.Count > 0);

                foreach (var audit in auditList)
                {
                    Console.WriteLine($"AUDIT: {audit.Event}: {audit.Entity}:{audit.EntityId} (of {audit.Aggregate}:{audit.AggregateId})");
                    Console.WriteLine(audit.Changes);
                    Console.WriteLine();
                }

                return auditList.Max(p => p.Id);
            }
        }
    }
}