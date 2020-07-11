using System;
using AppConfig;
using CoreTests.Helpers;
using Iti.Baseline.Core.DomainEventsBase;
using Iti.Baseline.Email;
using Iti.Baseline.Inversion;
using Iti.Baseline.Sms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreTests
{
    [Ignore]
    [TestClass]
    public class SmtpTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DefaultAppConfig.Initialize();
            IOC.RegisterType<IEmailSender, ConsoleEmailSender>();
            IOC.RegisterType<ISmsSender, ConsoleSmsSender>();

            IOC.RegisterType<IEmailRepository, TestEmailRepository>();
            IOC.RegisterType<ISmsRepository, TestSmsRepository>();

            DomainEvents.ClearRegistrations();
        }

        [TestMethod]
        public void SendEmailViaSmtp_ITI()
        {
            var sender = new SmtpEmailSender(new SmtpSettings
            {
                FromEmailAddress = "atlas@iticentral.com",
                FromDisplayName = "Support - Interface Technologies, Inc.",
                SmtpServer = "smtp.office365.com",
                SmtpPort = 587,
                SmtpUser = "atlas@iticentral.com",
                SmtpPassword = "TVWcT5anbFV8g3UL",
                Secure = true,
            });

            sender.Send(null, "todd@iticentral.com", $"ITI: Test - {DateTime.Now}", $"This is a test.  {DateTime.Now}");
        }

        [TestMethod]
        public void SendEmailViaSmtp_HpsPortal()
        {
            var sender = new SmtpEmailSender(new SmtpSettings
            {
                FromEmailAddress = "MedApproval@hospicepharmacysolutions.com",
                FromDisplayName = "Hospice Pharmacy Solutions",
                SmtpServer = "smtp.office365.com",
                SmtpPort = 587,
                SmtpUser = "MedApproval@hospicepharmacysolutions.com",
                SmtpPassword = "Vuz26072",
                Secure = true,
            });

            sender.Send(null, "todd@iticentral.com", $"HPSPORTAL: Test - {DateTime.Now}", $"This is a test.  {DateTime.Now}");
        }
    }
}