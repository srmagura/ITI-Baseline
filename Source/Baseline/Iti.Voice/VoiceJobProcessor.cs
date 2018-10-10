using System;
using Iti.Core.DateTime;
using Iti.Core.Services;
using Iti.Core.UnitOfWork;
using Iti.Logging;

namespace Iti.Voice
{
    public class VoiceJobProcessor : JobProcessor
    {
        private readonly VoiceJobSettings _settings;
        private readonly IVoiceSender _sender;
        private readonly IVoiceRepository _repo;

        public VoiceJobProcessor(VoiceJobSettings settings, IVoiceSender sender, IVoiceRepository repo)
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
                    _repo.ForEachPendingOrRetry(SendVoice);

                    CleanupVoice();

                    uow.Commit();
                }

            }
            catch (Exception exc)
            {
                Output("ERROR: VoiceJobProcessor.Run", exc);
                Log.Error("VoiceJobProcessor.Run", exc);
            }
        }

        private void CleanupVoice()
        {
            _repo.CleanupOldVoice(_settings.VoiceLifetimeDays);
        }

        private void SendVoice(VoiceRecord rec)
        {
            try
            {
                _sender.Send(rec.ToAddress, rec.Body);

                rec.Status = VoiceStatus.Sent;
                rec.SentUtc = DateTimeService.UtcNow;
            }
            catch (Exception exc)
            {
                Log.Error($"Could not send Voice record #{rec.Id}", exc);
                Output($"Could not send Voice record #{rec.Id}: EXCEPTION: {exc}");

                if (rec.RetryCount >= _settings.VoiceMaxRetries)
                {
                    rec.Status = VoiceStatus.Error;
                }
                else
                {
                    rec.RetryCount++;
                    rec.NextRetryUtc = DateTimeService.UtcNow.AddMinutes(_settings.VoiceRetryMinutes);
                }
            }
        }
    }
}