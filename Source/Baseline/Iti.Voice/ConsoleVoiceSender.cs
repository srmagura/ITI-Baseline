using System;
using Iti.Baseline.Identities;

namespace Iti.Baseline.Voice
{
    public class ConsoleVoiceSender : IVoiceSender
    {
        public void Send(NotificationId notificationId, string toPhoneNumber, string content)
        {
            Console.WriteLine($"=== Voice: {toPhoneNumber} ===============================");
            Console.WriteLine($"NOTIFICATION: {notificationId}");
            Console.WriteLine($"{content}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}
