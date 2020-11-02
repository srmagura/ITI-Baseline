using ITI.DDD.Application.UnitOfWorkBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Harness
{
    public static class UnitTestConfig
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize()
        {
            UnitOfWorkImpl.ShouldWaitForDomainEvents(true);
        }
    }
}
