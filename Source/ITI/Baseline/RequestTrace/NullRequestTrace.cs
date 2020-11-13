using System;
using System.Collections.Generic;
using System.Text;

namespace RequestTrace
{
    public class NullRequestTrace : IRequestTrace
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
            // do nothing
        }
    }
}
