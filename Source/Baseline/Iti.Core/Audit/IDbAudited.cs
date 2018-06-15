namespace Iti.Core.Audit
{
    public interface IDbAudited
    {
        string AuditEntityName { get; }
        string AuditEntityId { get; }
    }
}