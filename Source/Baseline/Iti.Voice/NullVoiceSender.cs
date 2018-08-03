namespace Iti.Voice
{
    public class NullVoiceSender : IVoiceSender
    {
        public void Send(string toPhoneNumber, string callbackUrl, string content)
        {
            // DO NOTHING
        }
    }
}