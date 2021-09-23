using ITI.Baseline.Audit;
using System.Threading.Tasks;

namespace TestApp.AppConfig
{
    public class AppPermissions : IAuditAppPermissions
    {
        public Task<bool> CanViewAuditAsync(string entityName, string entityId) => Task.FromResult(true);
    }
}
