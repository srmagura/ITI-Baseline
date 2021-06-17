﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITI.Baseline.Audit
{
    public interface IAuditAppService
    {
        Task<List<AuditRecordDto>> ListAsync(string entityName, string entityId, int skip, int take);
    }
}