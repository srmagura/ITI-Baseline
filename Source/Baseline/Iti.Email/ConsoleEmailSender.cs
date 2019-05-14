using System;
using Iti.Identities;

namespace Iti.Email
{
    public class ConsoleEmailSender : IEmailSender
    {
        public void Send(NotificationId notificationId, string toEmailAddress, string subject, string body)
        {
            Console.WriteLine("=== EMAIL ===============================");
            Console.WriteLine($"NOTIFICATION: {notificationId}");
            Console.WriteLine($"TO: {toEmailAddress}");
            Console.WriteLine($"SUBJECT: {subject}");
            Console.WriteLine($"{body}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}