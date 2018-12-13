namespace Iti.Sms
{
    public interface ISmsSender
    {
        void Send(long? notificationId, string toSmsAddress, string body);
    }
}