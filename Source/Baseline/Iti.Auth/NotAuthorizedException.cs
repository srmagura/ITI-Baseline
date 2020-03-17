using System;
using Iti.Exceptions;

namespace Iti.Auth
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