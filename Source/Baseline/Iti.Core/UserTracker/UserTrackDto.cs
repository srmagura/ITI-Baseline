using System;
using Iti.Core.DTOs;

namespace Iti.Core.UserTracker
{
    public class UserTrackDto : IDto
    {
        public DateTimeOffset LastAccessUtc { get; set; }
        public string UserId { get; set; }
        public string Service { get; set; }
    }
}