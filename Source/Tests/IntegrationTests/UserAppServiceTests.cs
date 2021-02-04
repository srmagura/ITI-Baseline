using IntegrationTests.Harness;
using ITI.DDD.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;

namespace IntegrationTests
{
    [TestClass]
    public class UserAppServiceTests
    {
        private static TestContext? TestContext;
        private IOC? _ioc;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            TestContext = context;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _ioc = IntegrationTestInitialize.Initialize(TestContext);
        }

        [TestMethod]
        public void GetCustomerUser()
        {
            var userSvc = _ioc!.Resolve<IUserAppService>();
            var customerId = Guid.NewGuid();

            var userId = userSvc.AddCustomerUser(
                customerId,
                new EmailAddressDto
                {
                    Value = "magura@example2.com"
                }
            );

            var user = userSvc.Get(userId);
            var customerUser = user as CustomerUserDto;
            
            Assert.IsNotNull(user);
            Assert.IsNotNull(customerUser);
            Assert.AreEqual("magura@example2.com", customerUser!.Email?.Value);
            Assert.AreEqual(customerId, customerUser.CustomerId.Guid);
        }

        [TestMethod]
        public void GetOnCallUser()
        {
            var userSvc = _ioc!.Resolve<IUserAppService>();
            var onCallProviderId = Guid.NewGuid();

            var userId = userSvc.AddOnCallUser(
                onCallProviderId,
                new EmailAddressDto
                {
                    Value = "todd@example2.com"
                }
            );

            var user = userSvc.Get(userId);
            var onCallUser = user as OnCallUserDto;

            Assert.IsNotNull(user);
            Assert.IsNotNull(onCallUser);
            Assert.AreEqual("todd@example2.com", onCallUser!.Email?.Value);
            Assert.AreEqual(onCallProviderId, onCallUser.OnCallProviderId.Guid);
        }

        [TestMethod]
        public void List()
        {
            var userSvc = _ioc!.Resolve<IUserAppService>();
            
            var customerId = Guid.NewGuid();
            var onCallProviderId = Guid.NewGuid();

            userSvc.AddCustomerUser(
                customerId,
                new EmailAddressDto
                {
                    Value = "magura@example2.com"
                }
            );
            userSvc.AddOnCallUser(
                onCallProviderId,
                new EmailAddressDto
                {
                    Value = "todd@example2.com"
                }
            );

            var users = userSvc.List();
            var customerUser = users.Single(u => u.Email?.Value == "magura@example2.com") as CustomerUserDto;
            var onCallUser = users.Single(u => u.Email?.Value == "todd@example2.com") as OnCallUserDto;

            Assert.IsNotNull(customerUser);
            Assert.AreEqual("magura@example2.com", customerUser!.Email?.Value);
            Assert.AreEqual(customerId, customerUser.CustomerId.Guid);

            Assert.IsNotNull(onCallUser);
            Assert.AreEqual("todd@example2.com", onCallUser!.Email?.Value);
            Assert.AreEqual(onCallProviderId, onCallUser.OnCallProviderId.Guid);
        }
    }
}
