using System;
using Iti.Core.DTOs;

namespace Iti.Core.Audit
{
    public class AuditRecordDto : IDto
    {
        public long Id { get; set; }
        public DateTimeOffset WhenUtc { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Aggregate { get; set; }
        public string AggregateId { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Event { get; set; }
        public string Changes { get; set; }
    }
}