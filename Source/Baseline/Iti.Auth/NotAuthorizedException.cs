using System;
using Iti.Baseline.Exceptions;

namespace Iti.Baseline.Auth
{
    public class NotAuthorizedException : DomainException
    {
        public NotAuthorizedException() 
            : base("Permission Denied", DomainException.AppServiceLogAs.None)
        {
        }

        public NotAuthorizedException(Exception innerException) 
            : base("Permission Denied", innerException, DomainException.AppServiceLogAs.None)
        {
        }
    }
}