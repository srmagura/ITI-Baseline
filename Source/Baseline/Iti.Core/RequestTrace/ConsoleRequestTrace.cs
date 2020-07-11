using System;
using Iti.Baseline.Core.DateTime;

namespace Iti.Baseline.Core.RequestTrace
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