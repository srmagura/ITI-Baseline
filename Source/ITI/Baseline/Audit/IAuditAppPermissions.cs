namespace ITI.Baseline.Audit
{
    public interface IAuditAppPermissions
    {
        Task<bool> CanViewAuditAsync(string entityName, string entityId);
    }
}