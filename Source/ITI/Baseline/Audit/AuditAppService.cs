using AutoMapper;
using ITI.Baseline.Util;
using ITI.DDD.Application;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Task<FilteredList<AuditRecordDto>> ListAsync(string entityName, string entityId, int skip, int take)
        {
            return QueryAsync(
                async () => Authorize.Require(await _perms.CanViewAuditAsync(entityName, entityId)),
                async () =>
                {
                    if (_context == null)
                        throw new Exception("IAuditDataContext not registered (IOC).");

                    var q = _context.AuditRecords
                        .Where(p => (p.Entity == entityName
                                     && p.EntityId == entityId
                                    )
                                    || (p.Aggregate == entityName
                                        && p.AggregateId == entityId
                                        && p.AggregateId != p.EntityId
                                    ));

                    var count = await q.CountAsync();

                    var items = await q
                        .OrderByDescending(p => p.WhenUtc)
                        .Skip(skip)
                        .Take(take)
                        .ToListAsync();

                    var dtos = _mapper.Map<List<AuditRecordDto>>(items);

                    return new FilteredList<AuditRecordDto>(count, dtos);
                }
            );

        }
    }
}