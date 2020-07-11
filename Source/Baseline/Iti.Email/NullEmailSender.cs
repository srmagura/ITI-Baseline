using Iti.Baseline.Identities;

namespace Iti.Baseline.Email
{
    public class NullEmailSender : IEmailSender
    {
        public void Send(NotificationId notificationId, string toEmailAddress, string subject, string body)
        {
            // DO NOTHING
        }
    }
}