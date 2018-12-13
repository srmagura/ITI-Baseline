using System;
using Iti.Core.DateTime;
using Iti.Core.Services;
using Iti.Core.UnitOfWork;
using Iti.Logging;

namespace Iti.Email
{
    public class EmailJobProcessor : JobProcessor
    {
        private readonly EmailJobSettings _settings;
        private readonly IEmailSender _sender;
        private readonly IEmailRepository _repo;

        public EmailJobProcessor(EmailJobSettings settings, IEmailSender sender, IEmailRepository repo)
        {
            _settings = settings;
            _sender = sender;
            _repo = repo;
        }

        public override void Run()
        {
            using (UnitOfWork.Begin())
            {
                SendEmail();
                Cleanup();
            }
        }

        private void SendEmail()
        {
            try
            {
                _repo.ForEachPendingOrRetry(SendEmail);
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void Cleanup()
        {
            try
            {
                _repo.CleanupOldEmails(_settings.EmailLifetimeDays);
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void SendEmail(EmailRecord rec)
        {
            try
            {
                _sender.Send(rec.NotificationId, rec.ToAddress, rec.Subject, rec.Body);

                rec.Status = EmailStatus.Sent;
                rec.SentUtc = DateTimeService.UtcNow;
            }
            catch (Exception exc)
            {
                Log.Error($"Could not send email record #{rec.Id}", exc);
                Output($"Could not send email record #{rec.Id}: EXCEPTION: {exc}");

                if (rec.RetryCount >= _settings.EmailMaxRetries)
                {
                    rec.Status = EmailStatus.Error;
                }
                else
                {
                    rec.RetryCount++;
                    rec.NextRetryUtc = DateTimeService.UtcNow.AddMinutes(_settings.EmailRetryMinutes);
                }
            }
        }
    }
}