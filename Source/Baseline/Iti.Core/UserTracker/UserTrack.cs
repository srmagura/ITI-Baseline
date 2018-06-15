using System;
using System.ComponentModel.DataAnnotations;
using Iti.Core.DateTime;
using Iti.Utilities;

namespace Iti.Core.UserTracker
{
    public class UserTrack
    {
        [Obsolete("Serialization use only")]
        protected UserTrack() { }

        public UserTrack(string userId, string service)
        {
            LastAccessUtc = DateTimeService.UtcNow;
            UserId = userId.MaxLength(128);
            Service = service.MaxLength(128);
        }

        //

        public long Id { get; protected set; }

        public DateTimeOffset LastAccessUtc { get; protected set; }

        [MaxLength(128)]
        public string UserId { get; protected set; }

        [MaxLength(128)]
        public string Service { get; protected set; }

        //

        public void Update(string service)
        {
            LastAccessUtc = DateTimeService.UtcNow;
            Service = service;
        }
    }
}