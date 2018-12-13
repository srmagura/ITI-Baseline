using System.Net;
using System.Net.Mail;

namespace Iti.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly ISmtpSettings _settings;

        public SmtpEmailSender(ISmtpSettings settings)
        {
            _settings = settings;
        }

        public void Send(long? notificationId, string toAddress, string subject, string body)
        {
            var smtp = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                EnableSsl = _settings.Secure,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            };

            // NOTE: do NOT use the object initializer for this (if you do, make SURE it as the LAST param)
            smtp.Credentials = new NetworkCredential(_settings.SmtpUser, _settings.SmtpPassword);

            var addrFrom = new MailAddress(_settings.FromEmailAddress, _settings.FromDisplayName);
            var addrTo = new MailAddress(toAddress, toAddress);

            subject = subject.Replace("\r", "");
            subject = subject.Replace("\n", " ");

            body = body + "\n\n";
            body = body.Replace("\r", "");
            body = body.Replace("\n", "<br/>");

            var message = new MailMessage(addrFrom, addrTo)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            smtp.Send(message);
        }
    }
}