using System;
using System.Collections.Generic;
using System.Linq;
using ITI.Baseline.Util;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ITI.Baseline.Audit
{
    public class Auditor : IAuditor
    {
        private readonly ILogger _logger;
        private readonly IAuthContext _auth;

        public Auditor(ILogger logger, IAuthContext auth)
        {
            _logger = logger;
            _auth = auth;
        }

        public void Process(DbContext context)
        {
            if (!(context is IAuditDataContext auditDataContext)) 
                return;

            try
            {
                var auditRecords = BuildAuditRecords(context.ChangeTracker);

                if (auditRecords.HasItems())
                {
                    auditDataContext.AuditRecords.AddRange(auditRecords);
                }
            }
            catch (Exception exc)
            {
                // eat exception!
                _logger?.Error("Could not process audit", exc);
            }
        }

        private List<AuditRecord> BuildAuditRecords(ChangeTracker changeTracker)
        {
            var list = new List<AuditRecord>();

            foreach (var entry in changeTracker.Entries())
            {
                try
                {
                    var entity = entry.Entity;

                    if (entity is IDbAudited auditEntity)
                    {
                        if (entry.State == EntityState.Detached)
                            continue;

                        //if (entry.State == EntityState.Unchanged && !HasReferenceChanges(entry))
                        //    continue;

                        var changeType = entry.State.ToString();
                        var changes = "";
                        //var changes = GetChangeDetails(auditEntity.AuditEntityName, entry);

                        var aggregateName = auditEntity.AuditEntityName;
                        var aggregateId = auditEntity.AuditEntityId;

                        if (entity is IDbAuditedChild child)
                        {
                            aggregateId = child.AuditAggregateId;
                            aggregateName = child.AuditAggregateName;

                            if (!child.HasParent)
                                changeType = EntityState.Deleted.ToString();
                        }

                        //if (ShouldAudit(changes))
                        //{
                            var audit = new AuditRecord(
                                _auth?.UserId, 
                                _auth?.UserName,
                                aggregateName, 
                                aggregateId,
                                auditEntity.AuditEntityName, 
                                auditEntity.AuditEntityId,
                                changeType, 
                                changes
                            );

                            list.Add(audit);
                        //}
                    }
                }
                catch (Exception exc)
                {
                    _logger?.Error("Audit Error", exc);
                }
            }

            return list;
        }
    }
}
