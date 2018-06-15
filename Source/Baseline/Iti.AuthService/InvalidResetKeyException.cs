﻿using System;
using Iti.Exceptions;

namespace Iti.AuthService
{
    public class InvalidResetKeyException : DomainException
    {
        public InvalidResetKeyException() : base("Invalid Password Reset Key")
        {
        }

        public InvalidResetKeyException(Exception innerException) : base("Invalid Password Reset Key", innerException)
        {
        }
    }
}