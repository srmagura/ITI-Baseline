namespace Iti.Sms
{
    public interface ITwilioSmsSettings
    {
        string Sid { get; }
        string AuthToken { get; }
        string FromPhone { get; }
    }
}