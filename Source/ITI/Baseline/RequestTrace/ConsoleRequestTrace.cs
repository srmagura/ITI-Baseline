namespace ITI.Baseline.RequestTrace;

public class ConsoleRequestTrace : IRequestTrace
{
    public void WriteTrace(
        string service,
        RequestTraceDirection direction,
        DateTimeOffset dateBeginUtc,
        string url,
        string request,
        string response,
        Exception? exc
    )
    {
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine($"SERVICE : {service}");
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
