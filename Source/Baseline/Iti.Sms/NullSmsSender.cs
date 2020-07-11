using Iti.Baseline.Identities;

namespace Iti.Baseline.Sms
{
    public class NullSmsSender : ISmsSender
    {
        public void Send(NotificationId notificationId, string toSmsAddress, string body)
        {
            // DO NOTHING
        }
    }
}