using System;
using Iti.Core.Entites;

namespace Iti.Identities
{
    public class NotificationId : Identity
    {
        public NotificationId() { }
        public NotificationId(Guid guid) : base(guid) { }
        public NotificationId(Guid? guid) : base(guid ?? default(Guid)) { }
    }
}
