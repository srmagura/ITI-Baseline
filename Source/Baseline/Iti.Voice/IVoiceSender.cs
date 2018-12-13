namespace Iti.Voice
{
    public interface IVoiceSender
    {
        void Send(long? notificationId, string toPhoneNumber, string content);
    }
}