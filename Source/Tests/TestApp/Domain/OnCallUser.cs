using ITI.Baseline.ValueObjects;
using System;
using TestApp.Domain.Enums;
using TestApp.Domain.Identities;

namespace TestApp.Domain
{
    public class OnCallUser : User
    {
        [Obsolete("Persistence use only")]
        protected OnCallUser() { }

        public OnCallUser(OnCallProviderId onCallProviderId, EmailAddress email)
            : base(UserType.OnCall, email)
        {
            OnCallProviderId = onCallProviderId;
        }

        public OnCallProviderId? OnCallProviderId { get; set; }
    }
}
