using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Iti.Auth;
using Iti.Core.Services;
using Iti.Core.UnitOfWorkBase.Interfaces;
using Iti.Logging;

namespace Iti.Core.Audit
{
    public class AuditAppService : ApplicationService, IAuditAppService
    {
        private readonly IAuditAppPermissions _perms;
        private readonly IAuditDataContext _context;

        public AuditAppService(IUnitOfWork uow, ILogger logger, IAuthContext baseAuth, IAuditAppPermissions perms, IAuditDataContext context)
            : base(uow, logger, baseAuth)
        {
            _perms = perms;
            _context = context;
        }

        public List<AuditRecordDto> List(string entityName, string entityId, int skip, int take)
        {
            try
            {
                Authorize.Require(_perms.CanViewAudit);

                if (_context == null)
                    throw new Exception("IAuditDataContext not registered (IOC)");

                var q = _context.AuditEntries
                    // .Where(p => p.Aggregate == entityName && p.AggregateId == entityId);
                    .Where(p => (p.Entity == entityName
                                 && p.EntityId == entityId
                                )
                                || (p.Aggregate == entityName
                                    && p.AggregateId == entityId
                                    && p.AggregateId != p.EntityId
                                    && (p.Event == "Added" || p.Event == "Deleted" || p.Event == "Removed")
                                ));

                var list = q
                    .OrderByDescending(p => p.WhenUtc)
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                return Mapper.Map<List<AuditRecordDto>>(list);
            }
            catch (Exception exc)
            {
                Log.Error("Could not retrieve audit records", exc);
                throw;
            }
        }
    }
}