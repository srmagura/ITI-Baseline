using ITI.Baseline.Util.Validation;
using ITI.Baseline.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Enums;
using TestApp.Domain.Identities;

namespace TestApp.Domain
{
    public class OnCallUser : User
    {
        [Obsolete("AutoMapper use only")]
        protected OnCallUser() { }

        public OnCallUser(OnCallProviderId onCallProviderId, EmailAddress email)
            : base(UserType.OnCall, email)
        {
            OnCallProviderId = onCallProviderId;
        }

        public OnCallProviderId? OnCallProviderId { get; set; }
    }
}
