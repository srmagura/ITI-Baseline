using System;
using Iti.Exceptions;

namespace Iti.AuthService
{
    public class LoginFailedException : DomainException
    {
        public LoginFailedException(string message) : base(message, DomainException.AppServiceLogAs.None)
        {
        }

        public LoginFailedException(string message, Exception innerException) : base(message, innerException, DomainException.AppServiceLogAs.None)
        {
        }
    }
}