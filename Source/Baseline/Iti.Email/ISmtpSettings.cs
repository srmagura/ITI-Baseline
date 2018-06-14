namespace Iti.Email
{
    public interface ISmtpSettings
    {
        string FromEmailAddress { get; }
        string FromDisplayName { get; }
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUser { get; }
        string SmtpPassword { get; }
        bool Secure { get; }
    }
}