namespace Iti.Email
{
    public interface IEmailSender
    {
        void Send(long? notificationId, string toEmailAddress, string subject, string body);
    }
}