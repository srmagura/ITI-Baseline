using System;
using Iti.Exceptions;

namespace Iti.Auth
{
    public class NotAuthenticatedException : DomainException
    {
        public NotAuthenticatedException() 
            : base("User not authenticated", DomainException.AppServiceLogAs.None)
        {
        }

        public NotAuthenticatedException(Exception innerException) 
            : base("User not authenticated", innerException, DomainException.AppServiceLogAs.None)
        {
        }
    }
}