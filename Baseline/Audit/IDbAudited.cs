namespace ITI.Baseline.Audit
{
    public interface IDbAudited
    {
        string AuditEntityName { get; }
        string AuditEntityId { get; }
    }
}