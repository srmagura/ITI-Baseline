using System;
using System.Collections.Generic;
using System.Linq;
using AppConfig;
using Autofac;
using CoreTests.Helpers;
using DataContext;
using Domain;
using Iti.Baseline.Auth;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Inversion;
using Iti.Baseline.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApp.Auth;

namespace CoreTests
{
    [TestClass]
    public class DbEntityTests
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
        public void UpdateAfterAddTest()
        {
            FooId fooId;
            string newName;

            var UnitOfWork = IOC.ResolveForTest<IUnitOfWork>();

            using (var uow = UnitOfWork.Begin())
            {
                var entity = new Foo($"FOO:{Guid.NewGuid()}", new List<Bar>(), new List<int>(), 100);

                var db = UnitOfWork.Current<SampleDataContext>();
                var dbEntity = DbEntity.ToDb<DbFoo>(entity);
                db.Foos.Add(dbEntity);

                newName = $"FOO_UPDATED:{Guid.NewGuid()}";
                entity.SetName(newName);

                uow.Commit();

                fooId = entity.Id;
            }

            using (var db = new SampleDataContext())
            {
                var back = db.Foos.FirstOrDefault(p => p.Id == fooId.Guid);
                Assert.IsNotNull(back);
                Assert.AreEqual(newName, back.Name);
            }
        }
    }
}