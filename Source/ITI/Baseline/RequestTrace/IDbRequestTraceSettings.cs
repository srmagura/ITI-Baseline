using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTrace
{
    public interface IDbRequestTraceSettings
    {
        string RequestTraceConnectionString { get; }
        string RequestTraceTableName { get; }
    }
}
