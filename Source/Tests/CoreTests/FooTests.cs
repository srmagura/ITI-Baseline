using System;
using System.Collections.Generic;
using System.Linq;
using AppConfig;
using Autofac;
using AutoMapper;
using CoreTests.Helpers;
using DataContext;
using Domain;
using Domain.Events;
using Iti.Baseline.Auth;
using Iti.Baseline.Core.Audit;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Inversion;
using Iti.Baseline.Logging;
using Iti.Baseline.Utilities;
using Iti.Baseline.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SampleApp.Application.Interfaces;
using SampleApp.Auth;

namespace CoreTests
{
    [TestClass]
    public class FooTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            DomainEvents.ClearRegistrations();

            IOC.RegisterType<IAuthContext, TestAuthContext>();
            IOC.RegisterType<IAppAuthContext, TestAuthContext>();

            IOC.RegisterType<ILogWriter, ConsoleLogWriter>();

            IOC.RegisterType<IAuditDataContext, SampleDataContext>();

            DomainEvents.Register<FooCreatedEvent, TestFooEventHandlers>();
            DomainEvents.Register<FooAddressChangedEvent, TestFooEventHandlers>();
            DomainEvents.Register<FooBarsChangedEvent, TestFooEventHandlers>();
        }

        [TestMethod]
        public void FooNotFoundIsNull()
        {
            var svc = GetFooService(null);

            var id = new FooId();
            var foo = svc.Get(id);
            Assert.IsNull(foo);
        }

        [TestMethod]
        public void Projections()
        {
            CreateFoo();    // make sure at least one!

            var svc = GetFooService(null);

            var list = svc.GetList();
            var fooId = list[0].Id;

            var fooRef = svc.ReferenceFor(fooId);
            Dump("REFERENCE", fooRef);

            var fooSum = svc.SummaryFor(fooId);
            Dump("SUMMARY", fooSum);
            Assert.IsTrue(fooSum.BarCount > 0);

            var junk = svc.JunkFor(fooId);
            Dump("JUNK", junk);
            Assert.AreEqual(junk.Ref.Id.Guid, junk.Summary.Id.Guid);
            Assert.AreEqual(junk.Ref.Name, junk.Summary.Name);
            Assert.IsTrue(junk.Summary.BarCount > 0);
        }

        [TestMethod]
        public void FooList()
        {
            CreateFoo();    // make sure at least one!

            var svc = GetFooService(null);

            var list = svc.GetList();
            Dump("Foo list", list);
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 1);
        }

        [TestMethod]
        public void CreateFoo()
        {
            TestFooEventHandlers.Clear();

            Console.WriteLine($"{TestFooEventHandlers.FooCreated} / {TestFooEventHandlers.FooAddressChanged} / {TestFooEventHandlers.FooBarsChanged}");

            var svc = GetFooService(null);

            var bars = new List<Bar>();
            bars.Add(new Bar(Guid.NewGuid().ToString()));
            bars.Add(new Bar(Guid.NewGuid().ToString()));
            bars.Add(new Bar(Guid.NewGuid().ToString()));

            var fooName = Guid.NewGuid().ToString();
            var fooId = svc.CreateFoo(fooName, bars);

            var foo = svc.Get(fooId);

            Dump("Foo out", foo);

            Assert.IsTrue(foo.SomeNumber > 0);

            Assert.AreEqual(fooId.Guid, foo.Id.Guid);

            var someIntList = foo.SomeInts.Split(',').Select(int.Parse).ToList();
            Assert.AreEqual(5, someIntList.Count);
            foreach (var i in new List<int> { 1, 3, 5, 7, 9 })
                Assert.AreEqual(1, someIntList.Count(p => p == i));
            Assert.IsNull(foo.SimpleAddress);

            svc.SetAddress(fooId, new SimpleAddress("1", "2", "3", "4", "5"));
            foo = svc.Get(fooId);
            Assert.IsNotNull(foo.SimpleAddress);
            Assert.AreEqual("1", foo.SimpleAddress.Line1);

            Assert.AreEqual(1, TestFooEventHandlers.FooCreated);
            Assert.AreEqual(1, TestFooEventHandlers.FooAddressChanged);
            Assert.AreEqual(0, TestFooEventHandlers.FooBarsChanged);

            Console.WriteLine($"{TestFooEventHandlers.FooCreated} / {TestFooEventHandlers.FooAddressChanged} / {TestFooEventHandlers.FooBarsChanged}");

            /* AUDIT TEST ... SHOULD MOVE */
            DumpAudit(fooId);
        }

        private static void DumpAudit(FooId fooId)
        {
            /*
            using (var db = new SampleDataContext())
            {
                var id = fooId.Guid.ToString();

                var audit = db.AuditEntries
                    .Where(p => p.Aggregate == "Foo" && p.AggregateId == id)
                    .OrderBy(p => p.Id)
                    .ToList();
                ;
                audit.ConsoleDump("AUDIT");
            }
            */
        }

        [TestMethod]
        public void AppService()
        {
            var svc = GetFooService(null);

            var bars = new List<Bar>();
            bars.Add(new Bar(Guid.NewGuid().ToString()));
            bars.Add(new Bar(Guid.NewGuid().ToString()));
            bars.Add(new Bar(Guid.NewGuid().ToString()));

            // CREATE FOO AND BARS

            var fooName = Guid.NewGuid().ToString();
            var fooId = svc.CreateFoo(fooName, bars,
                new SimpleAddress("4034 Winecott Drive", "", "Apex", "NC", "27502"),
                new SimplePersonName("Test", "Test", "Test"),
                new PhoneNumber("9198675309")
            );

            Console.WriteLine($"FOO ID: [{fooId}]");

            var foo = svc.Get(fooId);
            Dump("Foo out", foo);

            Assert.IsNotNull(foo);
            Assert.AreEqual(fooName, foo.Name);
            Assert.AreEqual(3, foo.Bars.Count);
            foreach (var bar in bars)
                Assert.IsTrue(foo.Bars.Any(p => p.Name == bar.Name));

            // CHANGE FOO NAME

            fooName = "Some New Name for this Foo!";
            svc.SetName(fooId, fooName);

            // CHANGE FOO ADDRESS

            svc.SetAddress(fooId, new SimpleAddress("x", "x", "x", "x", "x"));

            foo = svc.Get(fooId);
            Assert.AreEqual(fooName, foo.Name);
            Assert.AreEqual("x", foo.SimpleAddress.Line1);
            Assert.AreEqual("x", foo.SimpleAddress.Line2);
            Assert.AreEqual("x", foo.SimpleAddress.City);
            Assert.AreEqual("x", foo.SimpleAddress.State);
            Assert.AreEqual("x", foo.SimpleAddress.Zip);

            // REMOVE BAR

            foo = svc.Get(fooId);
            var barName = foo.Bars[0].Name;

            svc.RemoveBar(fooId, barName);

            foo = svc.Get(fooId);
            Dump("Get", foo);
            Assert.IsNotNull(foo);
            Assert.AreEqual(fooName, foo.Name);
            Assert.AreEqual(2, foo.Bars.Count);
            Assert.AreEqual(0, foo.Bars.Count(p => p.Name == barName));

            // ADD BAR

            svc.AddBar(fooId, "New Bar!");

            foo = svc.Get(fooId);
            Assert.IsNotNull(foo);
            Assert.AreEqual(fooName, foo.Name);
            Assert.AreEqual(3, foo.Bars.Count);
            Assert.AreEqual(1, foo.Bars.Count(p => p.Name == "New Bar!"));
            Dump("Final", foo);

            // CHANGE ALL BAR NAMES SAME

            svc.SetAllBarNames(fooId, "Same Name");

            foo = svc.Get(fooId);
            Assert.IsNotNull(foo);
            Assert.AreEqual(fooName, foo.Name);
            Assert.AreEqual(3, foo.Bars.Count);
            Assert.AreEqual(0, foo.Bars.Count(p => p.Name == "New Bar!"));
            Assert.AreEqual(3, foo.Bars.Count(p => p.Name.StartsWith("Same Name")));
            Dump("Final", foo);

            // REMOVE BAR

            foo = svc.Get(fooId);
            barName = foo.Bars[0].Name;

            foo.Bars.ConsoleDump();

            svc.RemoveBar(fooId, barName);

            foo = svc.Get(fooId);
            Dump("Get", foo);
            Assert.IsNotNull(foo);
            Assert.AreEqual(fooName, foo.Name);
            Assert.AreEqual(2, foo.Bars.Count);
            Assert.AreEqual(0, foo.Bars.Count(p => p.Name == barName));

            // REMOVE FOO

            svc.Remove(fooId);

            /* AUDIT TEST ... SHOULD MOVE */
            DumpAudit(fooId);
        }

        [TestMethod]
        public void TestIntListFromString()
        {
            var s = "";
            var list = ToList(s, int.Parse);
            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void TestGuidListFromString()
        {
            var s = "";
            var list = ToList(s, Guid.Parse);
            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void AddressMapTest()
        {
            var addr1 = new SimpleAddress("x", "x", "x", "x", "x");
            var addr2 = new SimpleAddress("y", "y", "y", "y", "y");

            Mapper.Map(addr1, addr2);
            Assert.AreEqual(addr1.City, addr2.City);
        }

        //
        //
        //

        private static IFooAppService GetFooService(ILifetimeScope scope)
        {
            if (scope == null)
                return IOC.ResolveForTest<IFooAppService>();

            return scope.Resolve<IFooAppService>();
        }

        private static List<T> ToList<T>(string s, Func<string, T> convert)
        {
            if (s == null || s.Trim() == "")
                return new List<T>();

            var list = s.Split(',').Select(convert).ToList();
            return list;
        }

        private void Dump(string tag, object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Console.WriteLine($"=== {tag}: {obj.GetType().Name} =====================");
            Console.WriteLine(json);
        }

    }
}
