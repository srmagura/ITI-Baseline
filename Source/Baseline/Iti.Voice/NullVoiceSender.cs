using Iti.Baseline.Identities;

namespace Iti.Baseline.Voice
{
    public class NullVoiceSender : IVoiceSender
    {
        public void Send(NotificationId notificationId, string toPhoneNumber, string content)
        {
            // DO NOTHING
        }
    }
}