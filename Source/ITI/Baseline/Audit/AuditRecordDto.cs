namespace ITI.Baseline.Audit
{
    public class AuditRecordDto
    {
        public AuditRecordDto(
            long id, 
            DateTimeOffset whenUtc, 
            string? userId, 
            string? userName, 
            string aggregate, 
            string aggregateId, 
            string entity, 
            string entityId, 
            string @event, 
            string changes
        )
        {
            Id = id;
            WhenUtc = whenUtc;
            UserId = userId;
            UserName = userName;
            Aggregate = aggregate ?? throw new ArgumentNullException(nameof(aggregate));
            AggregateId = aggregateId ?? throw new ArgumentNullException(nameof(aggregateId));
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            EntityId = entityId ?? throw new ArgumentNullException(nameof(entityId));
            Event = @event ?? throw new ArgumentNullException(nameof(@event));
            Changes = changes ?? throw new ArgumentNullException(nameof(changes));
        }

        public long Id { get; set; }
        public DateTimeOffset WhenUtc { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string Aggregate { get; set; }
        public string AggregateId { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Event { get; set; }
        public string Changes { get; set; }
    }
}