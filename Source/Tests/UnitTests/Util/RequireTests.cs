using ITI.Baseline.Util.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Util
{
    [TestClass]
    public class RequireTests
    {
        [TestMethod]
        public void NotNull_MakesInputObjectNotNull()
        {
            var version = Random.Shared.NextDouble() > -1 ? new Version("1.0.0") : null;
            Require.NotNull(version, "Object is null.");

            // Should not get a nullable warning here
            var minor = version.Minor;
            Assert.AreEqual(0, minor);
        }
    }
}
