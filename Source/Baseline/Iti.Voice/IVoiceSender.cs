namespace Iti.Voice
{
    public interface IVoiceSender
    {
        void Send(string toPhoneNumber, string content, string callbackUrl = null);
    }
}