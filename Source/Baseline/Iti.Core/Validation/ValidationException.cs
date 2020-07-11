using System;
using Iti.Baseline.Exceptions;

namespace Iti.Baseline.Core.Validation
{
    public class ValidationException : DomainException
    {
        public ValidationException(string message) : base(message, DomainException.AppServiceLogAs.None)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException, DomainException.AppServiceLogAs.None)
        {
        }
    }
}