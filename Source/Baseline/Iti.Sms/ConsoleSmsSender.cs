using System;
using Iti.Identities;

namespace Iti.Sms
{
    public class ConsoleSmsSender : ISmsSender
    {
        public void Send(NotificationId notificationId, string toSmsAddress, string body)
        {
            Console.WriteLine($"=== SMS: {toSmsAddress} ===============================");
            Console.WriteLine($"NOTIFICATION: {notificationId}");
            Console.WriteLine($"{body}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}