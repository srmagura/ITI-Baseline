using System;

namespace RequestTrace
{
    public class ConsoleRequestTrace : IRequestTrace
    {
        public void WriteTrace(
            string externalServiceName,
            RequestTraceDirection direction,
            DateTimeOffset dateBeginUtc,
            string url,
            string request,
            string response,
            Exception exc = null
        )
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine($"SERVICE : {externalServiceName}");
            Console.WriteLine($"DIR     : {direction}");
            Console.WriteLine($"BEGIN   : {dateBeginUtc}");
            Console.WriteLine($"END     : {DateTimeOffset.UtcNow}");
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