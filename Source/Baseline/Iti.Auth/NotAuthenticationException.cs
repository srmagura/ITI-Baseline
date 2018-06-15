using System;
using Iti.Exceptions;

namespace Iti.Auth
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