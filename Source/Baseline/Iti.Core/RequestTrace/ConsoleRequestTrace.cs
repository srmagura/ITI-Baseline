using System;

namespace Iti.Core.RequestTrace
{
    public class ConsoleRequestTrace : IRequestTrace
    {
        public void WriteTrace(string response)
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"REQUEST TRACE: {response}");
            Console.WriteLine("-----------------------------------------");
        }

        public void WriteTrace(string request, string response)
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"REQUEST TRACE: {request}");
            Console.WriteLine($"     Response: {response}");
            Console.WriteLine("-----------------------------------------");
        }
    }
}