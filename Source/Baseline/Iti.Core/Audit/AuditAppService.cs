using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iti.Auth;
using Iti.Core.Services;
using Iti.Inversion;
using Iti.Logging;

namespace Iti.Core.Audit
{
    public class AuditAppService : ApplicationService, IAuditAppService
    {
        private readonly IAuditAppPermissions _perms;

        public AuditAppService(IAuthContext baseAuth, IAuditAppPermissions perms)
            : base(baseAuth)
        {
            _perms = perms;
        }

        public List<AuditRecordDto> List(string entityName, string entityId, int skip, int take)
        {
            try
            {
                Authorize.Require(_perms.CanViewAudit);

                using (var db = IOC.TryResolve<IAuditDataContext>())
                {
                    if (db == null)
                        throw new Exception("IAuditDataContext not registered (IOC)");

                    var q = db.AuditEntries
                        .Where(p => p.Aggregate == entityName && p.AggregateId == entityId);

                    var list = q
                        .OrderByDescending(p => p.WhenUtc)
                        .Skip(skip)
                        .Take(take)
                        .ToList();

                    return Mapper.Map<List<AuditRecordDto>>(list);
                }
            }
            catch (Exception exc)
            {
                Log.Error("Could not retrieve audit records", exc);
                throw;
            }
        }
    }
}