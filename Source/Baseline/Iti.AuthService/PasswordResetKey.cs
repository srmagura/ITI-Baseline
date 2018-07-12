using System;
using System.ComponentModel.DataAnnotations;
using Iti.Core.DomainEvents;
using Iti.Core.Entites;
using Iti.Utilities;
using Iti.ValueObjects;

namespace Iti.AuthService
{
    public class PasswordResetKey : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected PasswordResetKey() { }

        public PasswordResetKey(EmailAddress email)
        {
            Key = Guid.NewGuid().ToString().MaxLength(128);
            Email = email;
        }

        //

        public PasswordResetKeyId Id { get; protected set; } = new PasswordResetKeyId();

        [MaxLength(128)]
        public string Key { get; protected set; }

        public EmailAddress Email { get; protected set; }
    }
}