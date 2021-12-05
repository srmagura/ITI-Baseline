using ITI.DDD.Logging;
using ITI.Baseline.RequestTrace;

namespace TestApp.DataContext;

public class ConnectionStrings : IDbLoggerSettings, IDbRequestTraceSettings
{
    public string AppDataContext { get; set; } = "Server=localhost;Database=ITIBaseline_e2e_test;Trusted_Connection=True;Connection Timeout=180;MultipleActiveResultSets=True;";

    public string LogConnectionString => AppDataContext;
    public string LogTableName => "LogEntries";

    public string RequestTraceConnectionString => AppDataContext;
    public string RequestTraceTableName => "RequestTraces";
}
