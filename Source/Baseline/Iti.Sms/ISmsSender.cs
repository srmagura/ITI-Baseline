namespace Iti.Sms
{
    public interface ISmsSender
    {
        void Send(string toSmsAddress, string body);
    }
}