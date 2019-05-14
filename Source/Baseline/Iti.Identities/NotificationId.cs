using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
