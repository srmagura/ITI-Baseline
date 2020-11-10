using ITI.Baseline.Audit;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.AppConfig
{
    public class AppPermissions : IAuditAppPermissions
    {
        public bool CanViewAudit => true;
    }
}
