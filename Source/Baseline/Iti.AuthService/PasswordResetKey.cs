using System;
using System.ComponentModel.DataAnnotations;
using Iti.Baseline.Core.DateTime;
using Iti.Baseline.Utilities;
using Iti.Baseline.ValueObjects;

namespace Iti.Baseline.AuthService
{
    public class PasswordResetKey
    {
        [Obsolete("Serialization use only")]
        protected PasswordResetKey() { }

        public PasswordResetKey(EmailAddress email)
        {
            Key = Guid.NewGuid().ToString().MaxLength(128);
            Email = email?.Value;

            DateCreatedUtc = DateTimeService.UtcNow;
        }

        //

        public long Id { get; set; }

        [MaxLength(128)]
        public string Key { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        public DateTimeOffset DateCreatedUtc { get; set; }
    }
}