using System;
using Iti.Baseline.Exceptions;

namespace Iti.Baseline.AuthService
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