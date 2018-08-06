using System;

namespace Iti.Core.RequestTrace
{
    public class NullRequestTrace : IRequestTrace
    {
        public void WriteTrace(DateTimeOffset dateBeginUtc, string url, string request, string response, Exception exc = null)
        {
            // DO NOTHING
        }
    }
}