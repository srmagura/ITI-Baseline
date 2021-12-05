using ITI.Baseline.Audit;

namespace TestApp.AppConfig;

public class AppPermissions : IAuditAppPermissions
{
    public Task<bool> CanViewAuditAsync(string entityName, string entityId) => Task.FromResult(true);
}
