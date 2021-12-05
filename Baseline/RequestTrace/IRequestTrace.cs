namespace ITI.Baseline.RequestTracing;

public interface IRequestTrace
{
    void WriteTrace(
        string service,
        RequestTraceDirection direction,
        DateTimeOffset dateBeginUtc,
        string url,
        string request,
        string response,
        Exception? exception = null
    );
}
