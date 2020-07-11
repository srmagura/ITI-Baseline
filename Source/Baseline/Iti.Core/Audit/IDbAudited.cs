namespace Iti.Baseline.Core.Audit
{
    public interface IDbAudited
    {
        string AuditEntityName { get; }
        string AuditEntityId { get; }
    }
}