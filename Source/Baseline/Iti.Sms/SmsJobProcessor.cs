using System;
using Iti.Core.DateTime;
using Iti.Core.Services;
using Iti.Core.UnitOfWork;
using Iti.Logging;

namespace Iti.Sms
{
    public class SmsJobProcessor : JobProcessor
    {
        private readonly SmsJobSettings _settings;
        private readonly ISmsSender _sender;
        private readonly ISmsRepository _repo;

        public SmsJobProcessor(SmsJobSettings settings, ISmsSender sender, ISmsRepository repo)
        {
            _settings = settings;
            _sender = sender;
            _repo = repo;
        }

        public override void Run()
        {
            try
            {
                using (var uow = UnitOfWork.Begin())
                {
                    _repo.ForEachPendingOrRetry(SendSms);

                    CleanupSms();

                    uow.Commit();
                }

            }
            catch (Exception exc)
            {
                Output("ERROR: SmsJobProcessor.Run", exc);
                Log.Error("SmsJobProcessor.Run", exc);
            }
        }

        private void CleanupSms()
        {
            _repo.CleanupOldSms(_settings.SmsLifetimeDays);
        }

        private void SendSms(SmsRecord rec)
        {
            try
            {
                _sender.Send(rec.ToAddress, rec.Body);

                rec.Status = SmsStatus.Sent;
                rec.SentUtc = DateTimeService.UtcNow;
            }
            catch (Exception exc)
            {
                Log.Error($"Could not send Sms record #{rec.Id}", exc);
                Output($"Could not send Sms record #{rec.Id}: EXCEPTION: {exc}");

                if (rec.RetryCount >= _settings.SmsMaxRetries)
                {
                    rec.Status = SmsStatus.Error;
                }
                else
                {
                    rec.RetryCount++;
                    rec.NextRetry = DateTimeService.UtcNow.AddMinutes(_settings.SmsRetryMinutes);
                }
            }
        }
    }
}