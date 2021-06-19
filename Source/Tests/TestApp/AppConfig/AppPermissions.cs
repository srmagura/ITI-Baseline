using ITI.Baseline.Audit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.AppConfig
{
    public class AppPermissions : IAuditAppPermissions
    {
        public Task<bool> CanViewAuditAsync(string entityName, string entityId) => Task.FromResult(true);
    }
}
