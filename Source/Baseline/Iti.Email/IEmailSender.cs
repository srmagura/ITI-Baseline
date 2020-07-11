using Iti.Baseline.Identities;

namespace Iti.Baseline.Email
{
    public interface IEmailSender
    {
        void Send(NotificationId notificationId, string toEmailAddress, string subject, string body);
    }
}