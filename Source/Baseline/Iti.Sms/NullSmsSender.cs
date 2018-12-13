namespace Iti.Sms
{
    public class NullSmsSender : ISmsSender
    {
        public void Send(long? notificationId, string toSmsAddress, string body)
        {
            // DO NOTHING
        }
    }
}