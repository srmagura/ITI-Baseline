namespace Iti.Voice
{
    public class QueuedVoiceSender : IVoiceSender
    {
        private readonly IVoiceRepository _repo;

        public QueuedVoiceSender(IVoiceRepository repo)
        {
            _repo = repo;
        }

        public void Send(string toVoiceAddress, string body, string callbackUrl = null)
        {
            // recordId is not used here (even if passed in)

            var rec = new VoiceRecord(toVoiceAddress, body);
            _repo.Add(rec);
        }
    }
}