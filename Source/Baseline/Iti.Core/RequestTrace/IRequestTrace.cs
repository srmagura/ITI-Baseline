using System;

namespace Iti.Baseline.Core.RequestTrace
{
    public interface IRequestTrace
    {
        void WriteTrace(DateTimeOffset dateBeginUtc, string url, string request, string response, Exception exc = null);
    }
}
