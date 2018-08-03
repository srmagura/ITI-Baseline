using System;

namespace Iti.Voice
{
    public class ConsoleVoiceSender : IVoiceSender
    {
        public void Send(string toPhoneNumber, string callbackUrl, string content)
        {
            Console.WriteLine($"=== Voice: {toPhoneNumber} ===============================");
            Console.WriteLine($"URL: [{callbackUrl}]");
            Console.WriteLine($"{content}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}
