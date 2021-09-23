using ITI.DDD.Application.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
