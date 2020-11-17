﻿using ITI.Baseline.Util.Validation;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Enums;
using TestApp.Domain.Identities;

namespace TestApp.Domain
{
    public abstract class User : AggregateRoot
    {
        protected User() { }

        protected User(UserType type, EmailAddress email)
        {
            Type = type;
            Email = email;
        }

        public UserId Id { get; set; } = new UserId();
        public UserType Type { get; set; }
        public EmailAddress? Email { get; set; }
    }
}
