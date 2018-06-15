using System;
using Iti.Exceptions;

namespace Iti.Authentication
{
    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException() : base("Invalid password")
        {
        }

        public InvalidPasswordException(Exception innerException) : base("Invalid password", innerException)
        {
        }
    }
}