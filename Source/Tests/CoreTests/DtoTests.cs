using System;
using System.Collections.Generic;
using System.Reflection;
using AppConfig;
using CoreTests.Helpers;
using Iti.Auth;
using Iti.Core.DomainEvents;
using Iti.Core.DTOs;
using Iti.Core.Mapping;
using Iti.Inversion;
using Iti.Logging;
using Iti.Utilities;
using Iti.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApp.Auth;

namespace CoreTests
{
    [TestClass]
    public class DtoTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            DomainEvents.ClearRegistrations();

            IOC.RegisterType<IAuthContext, TestAuthContext>();
            IOC.RegisterType<IAppAuthContext, TestAuthContext>();

            IOC.RegisterType<ILogWriter, ConsoleLogWriter>();
        }

        [TestMethod]
        public void NestedValueObjectsRemoveNoValueTest()
        {
            var dto = new VendorOrderDto
            {
                Phone = CreateInstance<PhoneNumber>(),
                Email = CreateInstance<EmailAddress>(),
                Stop1 = new VendorStopDto()
                {
                    Phone = CreateInstance<PhoneNumber>(),
                    Email = CreateInstance<EmailAddress>(),
                },
                Stops = new List<VendorStopDto>()
                {
                    new VendorStopDto()
                    {
                        Phone = CreateInstance<PhoneNumber>(),
                        Email = CreateInstance<EmailAddress>(),
                    },
                    new VendorStopDto()
                    {
                        Phone = CreateInstance<PhoneNumber>(),
                        Email = CreateInstance<EmailAddress>(),
                    },
                }
            };
            // dto.ConsoleDump();

            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Phone);
            Assert.IsNotNull(dto.Email);
            Verify(dto.Stop1, false);
            dto.Stops.ForEach(p => Verify(p, false));

            //

            BaseDataMapConfig.RemoveEmptyValueObjects(dto);
            dto.ConsoleDump();

            Assert.IsNotNull(dto);
            Assert.IsNull(dto.Phone);
            Assert.IsNull(dto.Email);
            Verify(dto.Stop1, true);
            dto.Stops.ForEach(p => Verify(p, true));
        }

        [TestMethod]
        public void NestedValueObjectsKeepWithValueTest()
        {
            var dto = new VendorOrderDto
            {
                Phone = new PhoneNumber("9192801222"),
                Email = new EmailAddress("x@x.com"),
                Stop1 = new VendorStopDto()
                {
                    Phone = new PhoneNumber("9192801222"),
                    Email = new EmailAddress("x@x.com"),
                },
                Stops = new List<VendorStopDto>()
                {
                    new VendorStopDto()
                    {
                        Phone = new PhoneNumber("9192801222"),
                        Email = new EmailAddress("x@x.com"),
                    },
                    new VendorStopDto()
                    {
                        Phone = new PhoneNumber("9192801222"),
                        Email = new EmailAddress("x@x.com"),
                    },
                }
            };
            // dto.ConsoleDump();

            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Phone);
            Assert.IsNotNull(dto.Email);
            Verify(dto.Stop1, false);
            dto.Stops.ForEach(p => Verify(p, false));

            //

            BaseDataMapConfig.RemoveEmptyValueObjects(dto);
            dto.ConsoleDump();

            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Phone);
            Assert.IsNotNull(dto.Email);
            Verify(dto.Stop1, false);
            dto.Stops.ForEach(p => Verify(p, false));
        }

        private void Verify(VendorStopDto dto, bool shouldBeNull)
        {
            Assert.IsNotNull(dto);
            Assert.AreEqual(shouldBeNull, dto.Phone == null);
            Assert.AreEqual(shouldBeNull, dto.Email == null);
        }

        private T CreateInstance<T>()
            where T : class
        {
            return Activator.CreateInstance(typeof(T),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new object[] { }, null) as T;
        }

        public class VendorOrderDto : IDto
        {
            public PhoneNumber Phone { get; set; }
            public EmailAddress Email { get; set; }

            public VendorStopDto Stop1 { get; set; }

            public List<VendorStopDto> Stops { get; set; }
        }

        public class VendorStopDto : IDto
        {
            public PhoneNumber Phone { get; set; }
            public EmailAddress Email { get; set; }
        }
    }
}