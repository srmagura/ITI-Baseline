using Iti.Identities;

namespace Iti.Voice
{
    public class NullVoiceSender : IVoiceSender
    {
        public void Send(NotificationId notificationId, string toPhoneNumber, string content)
        {
            // DO NOTHING
        }
    }
}