namespace Iti.Sms
{
    public class NullSmsSender : ISmsSender
    {
        public void Send(string toSmsAddress, string body)
        {
            // DO NOTHING
        }
    }
}