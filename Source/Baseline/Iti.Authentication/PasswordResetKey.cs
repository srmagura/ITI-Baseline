using System;
using System.ComponentModel.DataAnnotations;
using Iti.Core.DomainEvents;
using Iti.Core.Entites;
using Iti.Utilities;
using Iti.ValueObjects;

namespace Iti.Authentication
{
    public class PasswordResetKey : AggregateRoot
    {
        [Obsolete("Serialization use only")]
        protected PasswordResetKey() { }

        public PasswordResetKey(IAuthenticationUser authUser, EmailAddress email)
        {
            AuthUserId = authUser.IdAsString().MaxLength(FieldLengths.PasswordResetKey.AuthUserId);
            Key = Guid.NewGuid().ToString().MaxLength(FieldLengths.PasswordResetKey.Key);
            Email = email;

            DomainEvents.Raise(new PasswordResetKeyCreated(this));
        }

        //

        public PasswordResetKeyId Id { get; protected set; } = new PasswordResetKeyId();

        [MaxLength(FieldLengths.PasswordResetKey.AuthUserId)]
        public string AuthUserId { get; protected set; }

        [MaxLength(FieldLengths.PasswordResetKey.Key)]
        public string Key { get; protected set; }

        public EmailAddress Email { get; protected set; }
    }
}