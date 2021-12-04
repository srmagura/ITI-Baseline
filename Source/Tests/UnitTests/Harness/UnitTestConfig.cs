using ITI.DDD.Application;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Harness
{
    [TestClass]
    public static class UnitTestConfig
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            UnitOfWork.ShouldWaitForDomainEvents(true);
        }
    }
}
