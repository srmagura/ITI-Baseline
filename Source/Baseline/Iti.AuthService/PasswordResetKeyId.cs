using System;
using Iti.Baseline.Core.Entities;

namespace Iti.Baseline.AuthService
{
    public class PasswordResetKeyId : Identity
    {
        public PasswordResetKeyId() { }
        public PasswordResetKeyId(Guid guid) : base(guid) { }
    }
}