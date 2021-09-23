using System.Collections.Generic;

namespace ITI.Baseline.Audit
{
    public interface IAuditFieldConfiguration
    {
        // Key = entity name, Value = list of field names
        Dictionary<string, List<string>> IgnoredFields { get; }
        Dictionary<string, List<string>> MaskedFields { get; }
    }
}
