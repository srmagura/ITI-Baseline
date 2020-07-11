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
using Iti.Passwords;
using Iti.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApp.Auth;

namespace CoreTests
{
    [TestClass]
    public class PasswordTests
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
        }

        [TestMethod]
        public void EncoderTest()
        {
            var encoder = IOC.ResolveForTest<IPasswordEncoder<EncodedPassword>>();
            var enc = encoder.Encode("LetMeIn98");
            Assert.IsNotNull(enc);
            Assert.IsTrue(encoder.Validate("LetMeIn98", enc));
            enc.ConsoleDump();
        }

        [TestMethod]
        public void EncodePassword()
        {
            var pw = new DefaultPasswordEncoder(new DefaultPasswordEncoderSettings() { Pbkdf2Iterations = 3 });

            var isValid = pw.IsValid("LetMeIn98");
            Assert.IsTrue(isValid);

            var enc = pw.Encode("LetMeIn98");
            Console.WriteLine(enc);

            var validated = pw.Validate("LetMeIn98", enc);
            Assert.IsTrue(validated);
        }
    }
}