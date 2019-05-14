using Iti.Identities;

namespace Iti.Sms
{
    public interface ISmsSender
    {
        void Send(NotificationId notificationId, string toSmsAddress, string body);
    }
}