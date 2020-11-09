namespace ITI.Baseline.Audit
{
    public interface IDbAuditedChild : IDbAudited
    {
        string AuditAggregateName { get; }
        string AuditAggregateId { get; }

        bool HasParent { get; }
    }
}