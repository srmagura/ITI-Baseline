using System.Collections.Generic;

namespace Iti.Core.Audit
{
    public interface IAuditAppService
    {
        List<AuditRecordDto> List(string entityName, string entityId, int skip, int take);
    }
}