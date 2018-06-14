using System;

namespace Iti.Core.Exceptions
{
    public class NotAuthorizedException : DomainException
    {
        public NotAuthorizedException() 
            : base("Permission Denied")
        {
        }

        public NotAuthorizedException(Exception innerException) 
            : base("Permission Denied", innerException)
        {
        }
    }
}