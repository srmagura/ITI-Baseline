using Iti.Identities;

namespace Iti.Email
{
    public class NullEmailSender : IEmailSender
    {
        public void Send(NotificationId notificationId, string toEmailAddress, string subject, string body)
        {
            // DO NOTHING
        }
    }
}