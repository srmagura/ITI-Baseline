using System;

namespace Iti.Core.Exceptions
{
    public class NotAuthenticationException : DomainException
    {
        public NotAuthenticationException() 
            : base("User not authenticated")
        {
        }

        public NotAuthenticationException(Exception innerException) 
            : base("User not authenticated", innerException)
        {
        }
    }
}