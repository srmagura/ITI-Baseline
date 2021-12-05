using Autofac;
using IntegrationTests.Harness;
using ITI.Baseline.RequestTracing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestApp.DataContext;

namespace IntegrationTests
{
    [TestClass]
    public class RequestTraceTests : IntegrationTest
    {
        [TestMethod]
        public void WritesTrace()
        {
            var requestTrace = Container.Resolve<IRequestTrace>();

            requestTrace.WriteTrace(
                service: "Google",
                direction: RequestTraceDirection.Incoming,
                dateBeginUtc: DateTimeOffset.UtcNow.AddSeconds(-2),
                url: "myUrl",
                request: "myRequest",
                response: "myResponse",
                exception: new Exception("myException")
            );

            using var db = Container.Resolve<AppDataContext>();
            var trace = db.RequestTraces.Single();

            Assert.AreEqual("Google", trace.Service);
            Assert.AreEqual("Incoming", trace.Direction);
            Assert.AreEqual("myUrl", trace.Url);
            Assert.AreEqual("myRequest", trace.Request);
            Assert.AreEqual("myResponse", trace.Response);
            Assert.IsTrue(trace.Exception!.Contains("myException"));
        }
    }
}
