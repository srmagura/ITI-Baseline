using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.Baseline.RequestTrace
{
    public interface IDbRequestTraceSettings
    {
        string RequestTraceConnectionString { get; }
        string RequestTraceTableName { get; }
    }
}
