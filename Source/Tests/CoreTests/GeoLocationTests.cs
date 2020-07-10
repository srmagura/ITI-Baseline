using System;
using AppConfig;
using Autofac;
using CoreTests.Helpers;
using Iti.Auth;
using Iti.Core.DomainEventsBase;
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
            IOC.RegisterInstance(new GoogleGeoLocatorSettings() { ApiKey = "AIzaSyCHs9wcZRaJ8IUbLSqk5Aji5gmcrnu8jec" });

            _trace = new ConsoleRequestTrace();
        }

        private static IRequestTrace _trace;

        [TestMethod]
        public void UrlEscape()
        {
            var geo = new GoogleGeoLocator(new GoogleGeoLocatorSettings(), new Logger(new ConsoleLogWriter(), new TestAuthContext()), null);

            var s = geo.FormatAddressForUrl(new SimpleAddress("3401 E Lee Ave #b", "", "Yadkinville", "NC", "27055-99999"));
            Console.WriteLine(s);
        }

        [TestMethod]
        public void BasicTest()
        {
            var geo = IOC.Container.Resolve<IGeolocator>();

            var address = new SimpleAddress("4034 Winecott Drive", null, "Apex", "NC", "27502");
            address.ConsoleDump();

            var result = geo.Geocode(address);
            result.ConsoleDump();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.IsConfident);
            Assert.AreEqual(35.735, Math.Round(result?.Latitude ?? 0,3));
            Assert.AreEqual(-78.908, Math.Round(result?.Longitude ?? 0,3));
        }

        [TestMethod]
        public void PartialFindTest()
        {
            var geo = IOC.Container.Resolve<IGeolocator>();

            var address = new SimpleAddress("9999 Winecott Drive", null, "Apex", "NC", "27502");
            address.ConsoleDump();

            var result = geo.Geocode(address);
            result.ConsoleDump();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.IsFalse(result.IsConfident);
        }

        [Ignore]
        [TestMethod]
        public void NotFoundTest()
        {
            var geo = IOC.Container.Resolve<IGeolocator>();

            var address = new SimpleAddress("9999 Fooble Drive", null, "Apex", "XX", "27599");
            address.ConsoleDump();

            var result = geo.Geocode(address);
            result.ConsoleDump();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.IsFalse(result.IsConfident);
            Assert.AreEqual(null, result.Latitude.RoundTo(3));
            Assert.AreEqual(null, result.Longitude.RoundTo(3));
        }

        [TestMethod]
        public void WeirdAddressTest()
        {
            var geo = IOC.Container.Resolve<IGeolocator>();

            var address = new SimpleAddress("4034 Winecott Drive", "", "Apex", "NC", "27502");
            var result = geo.Geocode(address);
            result.ConsoleDump();

            address = new SimpleAddress("x", "x", "x", "WY", "x");
            result = geo.Geocode(address);
            result.ConsoleDump();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);  // really should be false !?
            Assert.IsTrue(result.IsConfident); // really should be false !?
        }

        [TestMethod]
        public void TimeZoneTest()
        {
            var geo = IOC.Container.Resolve<IGeolocator>();

            var address = new SimpleAddress("4034 Winecott Drive", "", "Apex", "NC", "27502");
            var loc = geo.Geocode(address);

            var tz = geo.TimezoneFor(loc);
            Assert.IsNotNull(tz);
            Assert.AreEqual("Eastern Standard Time", tz.Id);

            //

            address = new SimpleAddress("41325 Tollhouse Rd", "", "Shaver Lake", "CA", "93664");
            loc = geo.Geocode(address);

            tz = geo.TimezoneFor(loc);
            Assert.IsNotNull(tz);
            Assert.AreEqual("Pacific Standard Time", tz.Id);
        }

        [TestMethod]
        public void DistanceTest()
        {
            var geo = IOC.Container.Resolve<IGeolocator>();

            var from = new SimpleAddress("4034 Winecott Drive", "", "Apex", "NC", "27502");
            var to = new SimpleAddress("2435 Lynn Road", "Suite 206", "Raleigh", "NC", "27612");

            var dist = geo.GetDrivingDistance(from, to);
            Console.WriteLine($"DISTANCE: {dist}");

            Assert.IsTrue(dist > 24 && dist < 25);
        }
    }
}