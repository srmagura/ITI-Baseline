namespace Iti.Email
{
    public class NullEmailSender : IEmailSender
    {
        public void Send(long? notificationId, string toEmailAddress, string subject, string body)
        {
            // DO NOTHING
        }
    }
}