using AutoMapper;
using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITI.Baseline.Audit
{
    public class AuditAppService : ApplicationService, IAuditAppService
    {
        private readonly IAuditAppPermissions _perms;
        private readonly IAuditDataContext _context;
        private readonly IMapper _mapper;

        public AuditAppService(
            IUnitOfWork uow, 
            ILogger logger,
            IAuthContext auth, 
            IAuditAppPermissions perms,
            IAuditDataContext context,
            IMapper mapper
        ) : base(uow, logger, auth)
        {
            _perms = perms;
            _context = context;
            _mapper = mapper;
        }

        public List<AuditRecordDto> List(string entityName, string entityId, int skip, int take)
        {
            try
            {
                Authorize.Require(_perms.CanViewAudit);

                if (_context == null)
                    throw new Exception("IAuditDataContext not registered (IOC)");

                var q = _context.AuditRecords
                    .Where(p => (p.Entity == entityName
                                 && p.EntityId == entityId
                                )
                                || (p.Aggregate == entityName
                                    && p.AggregateId == entityId
                                    && p.AggregateId != p.EntityId)
                                );

                var list = q
                    .OrderByDescending(p => p.WhenUtc)
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                return _mapper.Map<List<AuditRecordDto>>(list);
            }
            catch (Exception exc)
            {
                Log.Error("Could not retrieve audit records", exc);
                throw;
            }
        }
    }
}