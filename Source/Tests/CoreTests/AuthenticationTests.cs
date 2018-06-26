using System;
using System.Collections.Generic;
using System.Linq;
using AppConfig;
using CoreTests.Helpers;
using Iti.Auth;
using Iti.AuthService;
using Iti.Core.DomainEvents;
using Iti.Email;
using Iti.Inversion;
using Iti.Logging;
using Iti.Passwords;
using Iti.Utilities;
using Iti.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleApp.Auth;

namespace CoreTests
{
    [TestClass]
    public class AuthenticationTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            using (new DebugTimer("App Configuration"))
            {
                DefaultAppConfig.Initialize();

                IOC.RegisterType<IAuthContext, TestAuthContext>();
                IOC.RegisterType<IAppAuthContext, TestAuthContext>();

                IOC.RegisterType<IAuthenticationService, BaseAuthenticationService>();
                IOC.RegisterType<IAuthenticationRepository, TestAuthRepo>();
                IOC.RegisterType<IAuthenticationUrlResolver, TestAuthUrlResolver>();

                IOC.RegisterType<IEmailSender, TestEmailSender>();
                IOC.RegisterType<ILogWriter, ConsoleLogWriter>();

                DomainEvents.ClearRegistrations();

                DomainEvents.Register<PasswordResetKeyCreatedEvent, PasswordResetKeyCreatedHandler>();
            }
        }

        [TestMethod]
        public void Login()
        {
            var svc = IOC.Resolve<IAuthenticationService>();

            var authId = svc.Login("1234", "LetMeIn98");
            Assert.IsNotNull(authId);
        }

        [TestMethod]
        public void PasswordReset()
        {
            var svc = IOC.Resolve<IAuthenticationService>();

            TestEmailSender.Sent.Clear();

            svc.RequestPasswordReset(new EmailAddress("todd@iticentral.com"));

            TestEmailSender.Sent.ConsoleDump();
            //Assert.AreEqual(1, TestEmailSender.Sent.Count);
        }
    }

    public class TestAuthUrlResolver : IAuthenticationUrlResolver
    {
        public string PasswordResetUrl(string resetKey)
        {
            return $"http://test.iticentral.com/Test/ResetPassword/{resetKey}";
        }
    }

    public class TestAuthUserId : IAuthenticationId
    {
        public TestAuthUserId(string s)
        {
            Id = s;
        }

        public string Id;
    }

    public class TestAuthUser : IAuthenticationUser
    {
        public IAuthenticationId Id => new TestAuthUserId("12341234");
        public string IdAsString()
        {
            return Id.ToString();
        }

        public bool IsActive { get; set; } = true;
        public EncodedPassword Password { get; set; } = new DefaultPasswordEncoder().Encode("LetMeIn98");
    }

    public class TestAuthRepo : IAuthenticationRepository
    {
        public IAuthenticationUser GetByLogin(string login)
        {
            return new TestAuthUser();
        }

        public IAuthenticationUser Get(IAuthenticationId id)
        {
            return new TestAuthUser();
        }

        public IAuthenticationUser Get(EmailAddress email)
        {
            return new TestAuthUser();
        }

        public IAuthenticationId AuthenticationIdFromString(string idString)
        {
            return new TestAuthUserId("12341234");
        }

        public void SetPassword(IAuthenticationId id, EncodedPassword encpw)
        {
            // do nothing
        }

        //
        //
        //

        public static List<PasswordResetKey> ResetKeys = new List<PasswordResetKey>();

        public void Add(PasswordResetKey pwrk)
        {
            Console.WriteLine($"ADD KEY: {pwrk.Key}");
            // pwrk.ConsoleDump();
            ResetKeys.Add(pwrk);
        }

        public PasswordResetKey GetPasswordResetKey(string resetKey)
        {
            Console.WriteLine($"GET KEY: {resetKey}");
            return ResetKeys.FirstOrDefault(p => p.Key == resetKey);
        }

        public void Remove(PasswordResetKeyId pwrkId)
        {
            ResetKeys.RemoveAll(p => p.Id.Guid == pwrkId.Guid);
        }
    }

    public class TestEmailSender : IEmailSender
    {
        public static List<dynamic> Sent = new List<dynamic>();

        public void Send(string toEmailAddress, string subject, string body)
        {
            Console.WriteLine("=== EMAIL ===============================");
            Console.WriteLine($"TO: {toEmailAddress}");
            Console.WriteLine($"SUBJECT: {subject}");
            Console.WriteLine($"{body}");
            Console.WriteLine("-----------------------------------------");

            Sent.Add(new
            {
                ToEmailAddress = toEmailAddress,
                Subject = subject,
                Body = body,
            });
        }
    }
}