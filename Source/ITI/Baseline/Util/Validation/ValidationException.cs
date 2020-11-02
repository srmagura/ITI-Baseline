using ITI.DDD.Core;
using System;

namespace ITI.Baseline.Util.Validation
{
    public class ValidationException : DomainException
    {
        public ValidationException(string message) 
            : base(message, DomainException.AppServiceLogAs.None)
        {
        }

        public ValidationException(string message, Exception innerException) 
            : base(message, innerException, DomainException.AppServiceLogAs.None)
        {
        }
    }
}