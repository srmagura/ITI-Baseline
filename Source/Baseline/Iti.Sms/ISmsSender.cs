using Iti.Baseline.Identities;

namespace Iti.Baseline.Sms
{
    public interface ISmsSender
    {
        void Send(NotificationId notificationId, string toSmsAddress, string body);
    }
}