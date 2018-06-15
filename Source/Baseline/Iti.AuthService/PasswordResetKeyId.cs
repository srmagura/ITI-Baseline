using System;
using Iti.Core.Entites;

namespace Iti.AuthService
{
    public class PasswordResetKeyId : Identity
    {
        public PasswordResetKeyId() { }
        public PasswordResetKeyId(Guid guid) : base(guid) { }
    }
}