namespace Iti.Voice
{
    public class NullVoiceSender : IVoiceSender
    {
        public void Send(long? notificationId, string toPhoneNumber, string content)
        {
            // DO NOTHING
        }
    }
}