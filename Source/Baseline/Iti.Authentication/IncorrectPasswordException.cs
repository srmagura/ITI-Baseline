using System;
using Iti.Exceptions;

namespace Iti.Authentication
{
    public class IncorrectPasswordException : DomainException
    {
        public IncorrectPasswordException() : base("Incorrect password")
        {
        }

        public IncorrectPasswordException(Exception innerException) : base("Incorrect password", innerException)
        {
        }
    }
}