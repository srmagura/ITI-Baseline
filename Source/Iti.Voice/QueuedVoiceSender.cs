namespace Iti.Voice
{
    public class QueuedVoiceSender : IVoiceSender
    {
        private readonly IVoiceRepository _repo;

        public QueuedVoiceSender(IVoiceRepository repo)
        {
            _repo = repo;
        }

        public void Send(string toVoiceAddress, string callbackUrl, string body)
        {
            var rec = new VoiceRecord(toVoiceAddress, callbackUrl, body);
            _repo.Add(rec);
        }
    }
}