namespace ITI.Baseline.RequestTrace
{
    public interface IDbRequestTraceSettings
    {
        string RequestTraceConnectionString { get; }
        string RequestTraceTableName { get; }
    }
}
