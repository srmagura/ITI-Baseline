namespace Iti.Email
{
    public interface IEmailSender
    {
        void Send(string toEmailAddress, string subject, string body);
    }
}