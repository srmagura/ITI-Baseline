using System;
using System.Collections.Generic;
using System.Linq;
using ITI.Baseline.Util;
using ITI.DDD.Auth;
using ITI.DDD.Core;
using ITI.DDD.Domain;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ITI.Baseline.Audit
{
    public class Auditor : IAuditor
    {
        private readonly ILogger _logger;
        private readonly IAuthContext _auth;
        private readonly IAuditFieldConfiguration _auditFieldConfiguration;

        public Auditor(ILogger logger, IAuthContext auth, IAuditFieldConfiguration auditFieldConfiguration)
        {
            _logger = logger;
            _auth = auth;
            _auditFieldConfiguration = auditFieldConfiguration;
        }

        public void Process(DbContext context)
        {
            if (context is not IAuditDataContext auditDataContext)
                return;

            try
            {
                var auditRecords = BuildAuditRecords(context.ChangeTracker);

                if (auditRecords.HasItems())
                {
                    auditDataContext.AuditRecords!.AddRange(auditRecords);
                }
            }
            catch (Exception exc)
            {
                // eat exception
                _logger?.Error("Could not process audit.", exc);
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

                        var changeType = entry.State == EntityState.Unchanged
                            ? EntityState.Modified.ToString()
                            : entry.State.ToString();

                        var changes = GetChangeDetails(auditEntity.AuditEntityName, entry);

                        var aggregateName = auditEntity.AuditEntityName;
                        var aggregateId = auditEntity.AuditEntityId;

                        if (entity is IDbAuditedChild child)
                        {
                            aggregateId = child.AuditAggregateId;
                            aggregateName = child.AuditAggregateName;

                            if (!child.HasParent)
                                changeType = EntityState.Deleted.ToString();
                        }

                        if (changes.HasValue() && changes != "[]")
                        {
                            var audit = new AuditRecord(
                                _auth?.UserIdString,
                                _auth?.UserName,
                                aggregateName,
                                aggregateId,
                                auditEntity.AuditEntityName,
                                auditEntity.AuditEntityId,
                                changeType,
                                changes
                            );

                            list.Add(audit);
                        }
                    }
                }
                catch (Exception exc)
                {
                    _logger?.Error("Audit Error", exc);
                }
            }

            return list;
        }

        private string GetChangeDetails(string entityName, EntityEntry entry)
        {
            var state = entry.State;

            if (entry.Entity is IDbAuditedChild child)
            {
                if (!child.HasParent)
                    state = EntityState.Deleted;
            }

            //

            var fromValues = entry.OriginalValues;
            var toValues = entry.CurrentValues;

            if (state == EntityState.Added || state == EntityState.Deleted)
                toValues = null;

            var auditProperties = GetAuditProperties(entityName, state, "", fromValues, toValues);

            //

            foreach (var reference in entry.References)
            {
                if (reference.TargetEntry?.Entity is DbValueObject)
                {
                    var valueObjectProps = GetValueObjectAuditProperties(entityName, reference);
                    auditProperties.AddRange(valueObjectProps);
                }
            }

            return auditProperties.ToDbJson();
        }

        private List<AuditPropertyDto> GetValueObjectAuditProperties(string entityName, ReferenceEntry reference, string? prefix = null)
        {
            if (reference.TargetEntry == null)
                throw new Exception("TargetEntry is null.");

            var state = reference.TargetEntry.State;

            var fromValues = reference.TargetEntry.OriginalValues;
            var toValues = reference.TargetEntry.CurrentValues;

            if (state == EntityState.Added || state == EntityState.Deleted)
                toValues = null;

            var auditProperties =  GetAuditProperties(entityName, state, prefix + "." + reference.Metadata.Name, fromValues, toValues);
            
            foreach(var childReference in reference.TargetEntry.References)
            {
                if (childReference.TargetEntry?.Entity is DbValueObject)
                {
                    var childProps = GetValueObjectAuditProperties(entityName, childReference, prefix: reference.Metadata.Name);
                    auditProperties.AddRange(childProps);
                }
            }

            return auditProperties;
        }

        private List<AuditPropertyDto> GetAuditProperties(
            string entityName,
            EntityState state,
            string prefix,
            PropertyValues fromValues,
            PropertyValues? toValues
        )
        {
            var auditProperties = new List<AuditPropertyDto>();

            foreach (var prop in fromValues.Properties)
            {
                if (prop.IsShadowProperty())
                    continue;

                if (prop.Name == "HasValue")
                    continue;

                var fieldName = prop.Name;
                var fromValue = fromValues[fieldName];
                var toValue = toValues?[fieldName];

                if (state == EntityState.Added)
                {
                    var tmp = fromValue;
                    fromValue = toValue;
                    toValue = tmp;
                }

                var changeInfo = GetAuditProperty(entityName, $"{prefix}.{fieldName}", fromValue, toValue);
                if (changeInfo != null)
                    auditProperties.Add(changeInfo);
            }

            return auditProperties;
        }

        private AuditPropertyDto? GetAuditProperty(string entityName, string fieldName, object? fromValue, object? toValue)
        {
            if (CompareValues(fromValue, toValue))
                return null;

            if (_auditFieldConfiguration.IgnoredFields.ContainsKey(entityName))
            {
                var ignoredFields = _auditFieldConfiguration.IgnoredFields[entityName];

                var fn = fieldName;
                while (fn.StartsWith("."))
                    fn = fn[1..];

                if (ignoredFields.Any(p => p == fn))
                {
                    return null;
                }
            }

            if (_auditFieldConfiguration.MaskedFields.ContainsKey(entityName))
            {
                var maskedFields = _auditFieldConfiguration.MaskedFields[entityName];

                var fn = fieldName;
                while (fn.StartsWith("."))
                    fn = fn[1..];

                if (maskedFields.Any(p => p == fn))
                {
                    if (fromValue != null)
                        fromValue = "(hidden)";
                    if (toValue != null)
                        toValue = "(hidden)";
                }
            }

            return new AuditPropertyDto(fieldName, fromValue?.ToString(), toValue?.ToString());
        }

        private static bool CompareValues(object? a, object? b)
        {
            if (a == null && b == null)
                return true;

            if (a == null || b == null)
                return false;

            if (a is decimal d1 && b is decimal d2)
            {
                return d1 == d2;
            }

            return a.ToString() == b.ToString();
        }
    }
}
