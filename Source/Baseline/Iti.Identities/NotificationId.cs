using System;
using Iti.Baseline.Core.Entites;

namespace Iti.Baseline.Identities
{
    public class NotificationId : Identity
    {
        public NotificationId() { }
        public NotificationId(Guid guid) : base(guid) { }
        public NotificationId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}
