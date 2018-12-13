using System;

namespace Iti.Sms
{
    public class ConsoleSmsSender : ISmsSender
    {
        public void Send(long? notificationId, string toSmsAddress, string body)
        {
            Console.WriteLine($"=== SMS: {toSmsAddress} ===============================");
            Console.WriteLine($"{body}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}