using Iti.Baseline.Identities;

namespace Iti.Baseline.Voice
{
    public interface IVoiceSender
    {
        void Send(NotificationId notificationId, string toPhoneNumber, string content);
    }
}