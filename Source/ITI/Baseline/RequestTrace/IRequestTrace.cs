using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTrace
{
    interface IRequestTrace
    {
        void WriteTrace(
            string externalServiceName, 
            RequestTraceDirection direction,
            DateTimeOffset dateBeginUtc, 
            string url, 
            string request, 
            string response, 
            Exception exc = null
        );
    }
}
