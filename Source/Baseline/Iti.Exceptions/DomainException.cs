using System;

namespace Iti.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message, bool appServiceShouldLog)
            : base(message)
        {
            AppServiceShouldLog = appServiceShouldLog;
        }

        public DomainException(string message, Exception innerException, bool appServiceShouldLog)
            : base(message, innerException)
        {
            AppServiceShouldLog = appServiceShouldLog;
        }

        public bool AppServiceShouldLog { get; }
    }
}