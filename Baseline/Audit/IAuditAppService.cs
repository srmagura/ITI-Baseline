using ITI.Baseline.Util;

namespace ITI.Baseline.Audit;

public interface IAuditAppService
{
    Task<FilteredList<AuditRecordDto>> ListAsync(string entityName, string entityId, int skip, int take);
}
