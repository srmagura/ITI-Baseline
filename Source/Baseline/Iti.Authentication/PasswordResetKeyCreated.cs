using Iti.Core.DomainEvents;

namespace Iti.Authentication
{
    public class PasswordResetKeyCreated : BaseDomainEvent
    {
        public PasswordResetKeyId PasswordResetKeyId { get; }
        public string Key { get; }

        public PasswordResetKeyCreated(PasswordResetKey pwrk)
        {
            PasswordResetKeyId = pwrk.Id;
            Key = pwrk.Key;
        }
    }
}