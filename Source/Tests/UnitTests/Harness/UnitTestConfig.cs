using ITI.DDD.Application.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Harness
{
    [TestClass]
    public static class UnitTestConfig
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            UnitOfWorkImpl.ShouldWaitForDomainEvents(true);
        }
    }
}
