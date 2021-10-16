using ITI.DDD.Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace ITI.Baseline.Audit
{
    // Make sure to call OnModelCreating to create an important index!
    public class AuditRecord
    {
        [Obsolete("Persistence use only")]
        protected AuditRecord() { }

        public AuditRecord(
            string? userId, 
            string? userName, 
            string aggregate, 
            string aggregateId, 
            string entity, 
            string entityId, 
            string eventName, 
            string changes
        )
        {
            WhenUtc = DateTimeOffset.UtcNow;
            UserId = userId?.MaxLength(64);
            UserName = userName?.MaxLength(64);
            Aggregate = aggregate.MaxLength(64);
            AggregateId = aggregateId.MaxLength(64);
            Entity = entity.MaxLength(64);
            EntityId = entityId.MaxLength(64);
            Event = eventName.MaxLength(64);
            Changes = changes;
        }

        public long Id { get; protected set; }

        public DateTimeOffset WhenUtc { get; protected set; }

        [MaxLength(64)]
        public string? UserId { get; protected set; }

        [MaxLength(64)]
        public string? UserName { get; protected set; }

        [MaxLength(64)]
        public string Aggregate { get; protected set; }

        [MaxLength(64)]
        public string AggregateId { get; protected set; }

        [MaxLength(64)]
        public string Entity { get; protected set; }

        [MaxLength(64)]
        public string EntityId { get; protected set; }

        [MaxLength(64)]
        public string Event { get; protected set; }

        public string Changes { get; protected set; }

        public static void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<AuditRecord>()
                .HasIndex(p => new { p.Entity, p.EntityId });

            mb.Entity<AuditRecord>()
                .HasIndex(p => new { p.Aggregate, p.AggregateId });
        }
    }
}