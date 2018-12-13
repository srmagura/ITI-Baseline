using System;

namespace Iti.Email
{
    public class ConsoleEmailSender : IEmailSender
    {
        public void Send(long? notificationId, string toEmailAddress, string subject, string body)
        {
            Console.WriteLine("=== EMAIL ===============================");
            Console.WriteLine($"TO: {toEmailAddress}");
            Console.WriteLine($"SUBJECT: {subject}");
            Console.WriteLine($"{body}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}