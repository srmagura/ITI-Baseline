using System;
using AppConfig;
using Autofac;
using CoreTests.Helpers;
using Iti.Baseline.Auth;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Core.Sequences;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Inversion;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class SequenceTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            IOC.RegisterType<IAuthContext, TestAuthContext>();

            DomainEvents.ClearRegistrations();
        }

        [TestMethod]
        public void BasicSequences()
        {
            using (var uow = IOC.ResolveForTest<IUnitOfWork>().Begin())
            {
                var seq = IOC.ResolveForTest<ISequenceResolver>();

                var x = seq.GetNextValue("Default");
                var y = seq.GetNextValue("Default");
                Console.WriteLine($"{x} ... {y}");

                Assert.IsTrue(x > 0);
                Assert.IsTrue(y == x + 1);

                x = seq.GetNextValue("OrderNumber");
                y = seq.GetNextValue("OrderNumber");
                Console.WriteLine($"{x} ... {y}");

                Assert.IsTrue(x > 0);
                Assert.IsTrue(y == x + 5);
            }
        }

    }
}