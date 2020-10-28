using ITI.DDD.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.DDD.Logging
{
    public class LogEntry
    {
        [Obsolete("Serialization use only")]
        protected LogEntry() { }

        public LogEntry(
            string level, 
            string? userId, 
            string? userName, 
            string hostname, 
            string process, 
            string? thread, 
            string message, 
            Exception? exception = null
        )
        {
            WhenUtc = DateTimeOffset.UtcNow;
            Level = level.MaxLength(16);
            UserId = userId?.MaxLength(128);
            UserName = userName?.MaxLength(128);
            Hostname = hostname.MaxLength(128);
            Process = process.MaxLength(128);
            Thread = thread?.MaxLength(128);
            Message = message;
            Exception = exception?.ToString();
        }

        //

        public long Id { get; protected set; }
        public DateTimeOffset WhenUtc { get; set; }

        [MaxLength(16)]
        public string? Level { get; protected set; }

        [MaxLength(128)]
        public string? UserId { get; protected set; }

        [MaxLength(128)]
        public string? UserName { get; protected set; }

        [MaxLength(128)]
        public string? Hostname { get; protected set; }

        [MaxLength(128)]
        public string? Process { get; protected set; }

        [MaxLength(128)]
        public string? Thread { get; protected set; }

        public string? Message { get; protected set; }
        public string? Exception { get; protected set; }
    }
}