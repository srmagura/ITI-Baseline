using Autofac;
using IntegrationTests.Harness;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TestApp.Application.Dto;
using TestApp.Application.Interfaces;
using TestApp.Domain.Identities;

namespace IntegrationTests
{
    [TestClass]
    public class UserAppServiceTests : IntegrationTest
    {
        [TestMethod]
        public async Task GetCustomerUser()
        {
            var userSvc = Container.Resolve<IUserAppService>();
            var customerId = new CustomerId();

            var userId = await userSvc.AddCustomerUserAsync(
                customerId,
                new EmailAddressDto
                {
                    Value = "magura@example2.com"
                }
            );

            var user = await userSvc.GetAsync(userId);
            var customerUser = user as CustomerUserDto;
            
            Assert.IsNotNull(user);
            Assert.IsNotNull(customerUser);
            Assert.AreEqual("magura@example2.com", customerUser!.Email?.Value);
            Assert.AreEqual(customerId, customerUser.CustomerId);
        }

        [TestMethod]
        public async Task GetOnCallUser()
        {
            var userSvc = Container.Resolve<IUserAppService>();
            var onCallProviderId = new OnCallProviderId();

            var userId = await userSvc.AddOnCallUserAsync(
                onCallProviderId,
                new EmailAddressDto
                {
                    Value = "todd@example2.com"
                }
            );

            var user = await userSvc.GetAsync(userId);
            var onCallUser = user as OnCallUserDto;

            Assert.IsNotNull(user);
            Assert.IsNotNull(onCallUser);
            Assert.AreEqual("todd@example2.com", onCallUser!.Email?.Value);
            Assert.AreEqual(onCallProviderId, onCallUser.OnCallProviderId);
        }

        [TestMethod]
        public async Task List()
        {
            var userSvc = Container.Resolve<IUserAppService>();
            
            var customerId = new CustomerId();
            var onCallProviderId = new OnCallProviderId();

            await userSvc.AddCustomerUserAsync(
                customerId,
                new EmailAddressDto
                {
                    Value = "magura@example2.com"
                }
            );
            await userSvc.AddOnCallUserAsync(
                onCallProviderId,
                new EmailAddressDto
                {
                    Value = "todd@example2.com"
                }
            );

            var users = await userSvc.ListAsync();
            var customerUser = users.Single(u => u.Email?.Value == "magura@example2.com") as CustomerUserDto;
            var onCallUser = users.Single(u => u.Email?.Value == "todd@example2.com") as OnCallUserDto;

            Assert.IsNotNull(customerUser);
            Assert.AreEqual("magura@example2.com", customerUser!.Email?.Value);
            Assert.AreEqual(customerId, customerUser.CustomerId);

            Assert.IsNotNull(onCallUser);
            Assert.AreEqual("todd@example2.com", onCallUser!.Email?.Value);
            Assert.AreEqual(onCallProviderId, onCallUser.OnCallProviderId);
        }
    }
}
