using System;
using Iti.Exceptions;

namespace Iti.AuthService
{
    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException() : base("Invalid password", false)
        {
        }

        public InvalidPasswordException(Exception innerException) : base("Invalid password", innerException, false)
        {
        }
    }
}