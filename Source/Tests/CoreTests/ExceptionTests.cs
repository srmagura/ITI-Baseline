using System;
using Iti.Core.Services;
using Iti.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        public void DbUpdateExceptionFormat()
        {
            var excMsg =
                "Cannot insert duplicate key row in object 'dbo.RegionPostalCodes' with unique index 'RegionPostalCodeUnique'. The duplicate key value is (10001).";

            ApplicationService.GetDbUpdateInfo(excMsg, out var table, out var value);
            Assert.AreEqual("RegionPostalCodes", table);
            Assert.AreEqual("10001", value);
        }

        [TestMethod]
        public void AppServiceHandlesDbUpdateException()
        {
            var svc = new TestAppSvc();

            try
            {
                var obj = svc.SomeQuery();
                Assert.Fail();
            }
            catch (DuplicateKeyException exc)
            {
                Console.WriteLine("======");
                Console.WriteLine(exc);

                Assert.AreEqual("RegionPostalCodes", exc.Table);
                Assert.AreEqual("10001", exc.Value);
                Assert.AreEqual(DomainException.AppServiceLogAs.None, exc.AppServiceShouldLog);
                Assert.IsNotNull(exc.InnerException);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Was expecting DuplicateKeyException, but got {ex.GetType().Name}");
            }

            try
            {
                svc.SomeCommand();
                Assert.Fail();
            }
            catch (DuplicateKeyException exc)
            {
                Console.WriteLine("======");
                Console.WriteLine(exc);

                Assert.AreEqual("RegionPostalCodes", exc.Table);
                Assert.AreEqual("10001", exc.Value);
                Assert.AreEqual(DomainException.AppServiceLogAs.None, exc.AppServiceShouldLog);
                Assert.IsNotNull(exc.InnerException);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Was expecting DuplicateKeyException, but got {ex.GetType().Name}");
            }
        }

        private class TestAppSvc : ApplicationService
        {
            public TestAppSvc()
                : base(null)
            {
            }

            public object SomeQuery()
            {
                return Query(() => { },
                    () =>
                    {
                        CauseDbUpdateException();
                        Assert.Fail();
                        return new object();
                    });
            }

            public void SomeCommand()
            {
                Command(() => { },
                    () =>
                    {
                        CauseDbUpdateException();
                        Assert.Fail();
                    });
            }

            private void CauseDbUpdateException()
            {
                var innerException = new Exception(@"Cannot insert duplicate key row in object 'dbo.RegionPostalCodes' with unique index 'RegionPostalCodeUnique'. The duplicate key value is (10001).");
                throw new DbUpdateException("This is the outer exception", innerException);
            }
        }
    }
}