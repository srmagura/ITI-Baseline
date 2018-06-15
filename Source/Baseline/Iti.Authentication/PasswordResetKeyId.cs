using System;
using Iti.Core.Entites;

namespace Iti.Authentication
{
    public class PasswordResetKeyId : Identity
    {
        public PasswordResetKeyId() { }
        public PasswordResetKeyId(Guid guid) : base(guid) { }
    }
}