using Iti.Identities;

namespace Iti.Sms
{
    public class NullSmsSender : ISmsSender
    {
        public void Send(NotificationId notificationId, string toSmsAddress, string body)
        {
            // DO NOTHING
        }
    }
}