using Iti.Core.DomainEvents;

namespace Iti.Authentication
{
    public class PasswordResetKeyCreatedEvent : BaseDomainEvent
    {
        public PasswordResetKeyId PasswordResetKeyId { get; }
        public string Key { get; }

        public PasswordResetKeyCreatedEvent(PasswordResetKey pwrk)
        {
            PasswordResetKeyId = pwrk.Id;
            Key = pwrk.Key;
        }
    }
}