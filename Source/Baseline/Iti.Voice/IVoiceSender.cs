using Iti.Identities;

namespace Iti.Voice
{
    public interface IVoiceSender
    {
        void Send(NotificationId notificationId, string toPhoneNumber, string content);
    }
}