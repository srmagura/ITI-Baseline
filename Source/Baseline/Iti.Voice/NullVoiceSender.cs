namespace Iti.Voice
{
    public class NullVoiceSender : IVoiceSender
    {
        public void Send(string toPhoneNumber, string content, string callbackUrl = null)
        {
            // DO NOTHING
        }
    }
}