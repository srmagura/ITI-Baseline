using Iti.Baseline.Identities;

namespace Iti.Baseline.Voice
{
    public class QueuedVoiceSender : IVoiceSender
    {
        private readonly IVoiceRepository _repo;

        public QueuedVoiceSender(IVoiceRepository repo)
        {
            _repo = repo;
        }

        public void Send(NotificationId notificationId, string toVoiceAddress, string body)
        {
            var rec = new VoiceRecord(notificationId, toVoiceAddress, body);
            _repo.Add(rec);
        }
    }
}