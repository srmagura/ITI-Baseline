using ITI.DDD.Logging;
using ITI.Baseline.RequestTrace;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.DataContext
{
    public class ConnectionStrings : IDbLoggerSettings, IDbRequestTraceSettings
    {
        public string DefaultDataContext { get; set; } = "Server=localhost;Database=ITIBaseline_e2e_test;Trusted_Connection=True;Connection Timeout=180;MultipleActiveResultSets=True;";

        public string LogConnectionString => DefaultDataContext;
        public string LogTableName => "LogEntries";

        public string RequestTraceConnectionString => DefaultDataContext;
        public string RequestTraceTableName => "RequestTraces";
    }
}
