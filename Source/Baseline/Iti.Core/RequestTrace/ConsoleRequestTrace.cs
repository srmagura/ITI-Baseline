using System;
using Iti.Core.DateTime;

namespace Iti.Core.RequestTrace
{
    public class ConsoleRequestTrace : IRequestTrace
    {
        public void WriteTrace(DateTimeOffset dateBeginUtc, string url, string request, string response, Exception exc = null)
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"BEGIN   : {dateBeginUtc}");
            Console.WriteLine($"END     : {DateTimeService.UtcNow}");
            Console.WriteLine($"URL     : {url}");
            Console.WriteLine($"REQUEST : {request}");
            Console.WriteLine($"RESPONSE: {response}");
            if (exc != null)
            {
                Console.WriteLine($"EXCEPTION: {exc}");
            }
            Console.WriteLine("-----------------------------------------");
        }
    }
}