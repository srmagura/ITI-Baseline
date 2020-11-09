using System.Collections.Generic;

namespace ITI.Baseline.Audit
{
    public interface IAuditAppService
    {
        List<AuditRecordDto> List(string entityName, string entityId, int skip, int take);
    }
}