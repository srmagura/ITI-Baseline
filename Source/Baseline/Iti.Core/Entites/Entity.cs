using System;
using Iti.Core.DateTime;

namespace Iti.Core.Entites
{
    public abstract class Entity
    {
        protected Entity()
        {
            DateCreatedUtc = DateTimeService.UtcNow;
        }

        public DateTimeOffset DateCreatedUtc { get; protected set; }
    }
}