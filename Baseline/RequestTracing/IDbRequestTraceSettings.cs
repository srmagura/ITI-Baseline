namespace ITI.Baseline.RequestTracing;

public interface IDbRequestTraceSettings
{
    string RequestTraceConnectionString { get; }
    string RequestTraceTableName { get; }
}
