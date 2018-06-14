namespace Iti.Email
{
    public class NullEmailSender : IEmailSender
    {
        public void Send(string toEmailAddress, string subject, string body)
        {
            // DO NOTHING
        }
    }
}