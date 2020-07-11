using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using AppConfig;
using Autofac;
using AutoMapper;
using CoreTests.Helpers;
using DataContext;
using Iti.Baseline.Auth;
using Iti.Baseline.Core.Audit;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Core.Entites;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Inversion;
using Iti.Baseline.Logging;
using Iti.Baseline.ValueObjects;
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

            Auditor.MaskField("foo", "someNumber");
        }

        [TestMethod]
        public void ValueObjectTest()
        {
            long auditId = 0;

            var fooId = SequentialGuid.Next();

            var nullAddr = Activator.CreateInstance(typeof(SimpleAddress),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new object[] { }, null) as SimpleAddress;
            var nullPhone = Activator.CreateInstance(typeof(PhoneNumber),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new object[] { }, null) as PhoneNumber;
            var nullName = Activator.CreateInstance(typeof(SimplePersonName),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new object[] { }, null) as SimplePersonName;

            var addr1 = new SimpleAddress("1", "1", "1", "1", "1");
            var addr2 = new SimpleAddress("2", "2", "2", "2", "2");

            Section("CREATE");

            var UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();

                var foo = new DbFoo()
                {
                    Id = fooId,
                    Name = "Created",
                    NotInEntity = "Foo!",
                    SimpleAddress = nullAddr,
                    SimplePersonName = nullName,
                    PhoneNumber = nullPhone,
                };

                db.Foos.Add(foo);

                // db.SaveChanges();
                uow.Commit();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("MODIFY");
            UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();
            
                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                foo.SomeNumber = 1234;

                // db.SaveChanges();
                uow.Commit();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("SET ADDRESS 1");

            UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();
                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                Assert.IsNull(foo.SimpleAddress.City);

                // foo.Name = "Set Address 1";

                // foo.Address = addr1;
                Mapper.Map(addr1, foo.SimpleAddress);

                // db.SaveChanges();
                uow.Commit();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("SET ADDRESS 2");

            UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();
                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                Assert.AreEqual("1", foo.SimpleAddress.City);

                // foo.Name = "Set Address 2";

                // foo.Address = addr2;
                Mapper.Map(addr2, foo.SimpleAddress);

                // db.SaveChanges();
                uow.Commit();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("Read Back");

            UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();
                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                Assert.AreEqual("2", foo.SimpleAddress.City);
            }

            Section("Add Bar");

            UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();

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

                // db.SaveChanges();
                uow.Commit();

                AssertBarCount(fooId, 2);

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("Remove Bar");

            UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();

                var foo = db.Foos
                    .Include(p => p.Bars)
                    .FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                foo.Bars.RemoveAll(p => p.NotInEntity == "RemoveMe!");

                // db.SaveChanges();
                uow.Commit();

                AssertBarCount(fooId, 1);

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }

            Section("Remove");

            UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();

                var foo = db.Foos.FirstOrDefault(p => p.Id == fooId);
                Assert.IsNotNull(foo);

                db.Foos.Remove(foo);

                // db.SaveChanges();
                uow.Commit();

                var nextAuditId = DumpAudit(fooId, auditId);
                Assert.AreNotEqual(auditId, nextAuditId);
                auditId = nextAuditId;
            }
        }

        private void AssertBarCount(Guid fooId, int expectedCount)
        {
            var UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();

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
            var UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var db = UnitOfWork.Current<SampleDataContext>();

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