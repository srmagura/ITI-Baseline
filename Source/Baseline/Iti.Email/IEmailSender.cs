using Iti.Identities;

namespace Iti.Email
{
    public interface IEmailSender
    {
        void Send(NotificationId notificationId, string toEmailAddress, string subject, string body);
    }
}