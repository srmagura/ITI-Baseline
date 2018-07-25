using AppConfig;
using CoreTests.Helpers;
using Iti.Auth;
using Iti.Core.DomainEvents;
using Iti.Core.RequestTrace;
using Iti.Geolocation;
using Iti.Inversion;
using Iti.Logging;
using Iti.Utilities;
using Iti.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApp.Auth;

namespace CoreTests
{
    [TestClass]
    public class GeoLocationTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            DomainEvents.ClearRegistrations();

            IOC.RegisterType<IAuthContext, TestAuthContext>();
            IOC.RegisterType<IAppAuthContext, TestAuthContext>();

            IOC.RegisterType<ILogWriter, ConsoleLogWriter>();

            // TODO:JT:XXX: need an ITI google api key
            IOC.RegisterInstance(new GoogleGeoLocatorSettings() { ApiKey = "AIzaSyCCQVFpJfK8jH4hVvjAjnx_j1QNcM3QA3s" });

            _trace = new ConsoleRequestTrace();
        }

        private static IRequestTrace _trace;

        [TestMethod]
        public void BasicTest()
        {
            var geo = IOC.Resolve<IGeolocator>();

            var address = new Address("4034 Winecott Drive", null, "Apex", "NC", "27502");
            address.ConsoleDump();

            var result = geo.Geocode(address, _trace);
            result.ConsoleDump();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.IsConfident);
            Assert.AreEqual(35.735m, result.Latitude.RoundTo(3));
            Assert.AreEqual(-78.908m, result.Longitude.RoundTo(3));
        }

        [TestMethod]
        public void PartialFindTest()
        {
            var geo = IOC.Resolve<IGeolocator>();

            var address = new Address("9999 Winecott Drive", null, "Apex", "NC", "27502");
            address.ConsoleDump();

            var result = geo.Geocode(address, _trace);
            result.ConsoleDump();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.IsConfident);
        }

        [TestMethod]
        public void NotFoundTest()
        {
            var geo = IOC.Resolve<IGeolocator>();

            var address = new Address("9999 Fooble Drive", null, "Apex", "XX", "27599");
            address.ConsoleDump();

            var result = geo.Geocode(address, _trace);
            result.ConsoleDump();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.IsFalse(result.IsConfident);
            Assert.AreEqual(0, result.Latitude.RoundTo(3));
            Assert.AreEqual(0, result.Longitude.RoundTo(3));
        }

        [TestMethod]
        public void WeirdAddressTest()
        {
            var geo = IOC.Resolve<IGeolocator>();

            var address = new Address("4034 Winecott Drive", "", "Apex", "NC", "27502");
            var result = geo.Geocode(address, _trace);
            result.ConsoleDump();

            address = new Address("x","x","x","WY","x");
            result = geo.Geocode(address, _trace);
            result.ConsoleDump();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.IsFalse(result.IsConfident);
            Assert.AreEqual(0, result.Latitude.RoundTo(3));
            Assert.AreEqual(0, result.Longitude.RoundTo(3));
        }
    }
}