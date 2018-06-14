using System;
using AppConfig;
using Iti.Core.DomainEvents;
using Iti.Core.Sequences;
using Iti.Core.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests
{
    [TestClass]
    public class SequenceTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            DomainEvents.ClearRegistrations();
        }

        [TestMethod]
        public void BasicSequences()
        {
            using (UnitOfWork.Begin())
            {
                var x = Sequence.Next();
                var y = Sequence.Next();
                Console.WriteLine($"{x} ... {y}");

                Assert.IsTrue(x > 0);
                Assert.IsTrue(y == x + 1);

                x = Sequence.Next("OrderNumber");
                y = Sequence.Next("OrderNumber");
                Console.WriteLine($"{x} ... {y}");

                Assert.IsTrue(x > 0);
                Assert.IsTrue(y == x + 5);
            }
        }

    }
}