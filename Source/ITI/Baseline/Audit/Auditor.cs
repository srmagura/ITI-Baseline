﻿using System;
using System.Collections.Generic;
using System.Linq;
using ITI.Baseline.Util;
using ITI.DDD.Application;
using ITI.DDD.Auth;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

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

            ////

            //foreach (var rf in entry.References)
            //{
            //    if (rf?.TargetEntry?.Entity is IValueObject)
            //    {
            //        AddValueObject(entityName, rf, auditProperties);
            //    }
            //}

            return auditProperties.ToJson(Formatting.None);
        }

        private List<AuditPropertyDto> GetAuditProperties(
            string entityName,
            EntityState state,
            string prefix,
            PropertyValues fromValues,
            PropertyValues toValues
        )
        {
            var auditProperties = new List<AuditPropertyDto>();

            foreach (var prop in fromValues.Properties)
            {
                if (prop.IsShadowProperty())
                    continue;

                var fieldName = prop.Name;
                var fromValue = fromValues[fieldName];
                var toValue = toValues?[fieldName];

                var changeInfo = GetAuditProperty(entityName, state, $"{prefix}.{fieldName}", fromValue, toValue);
                if (changeInfo != null)
                    auditProperties.Add(changeInfo);
            }

            return auditProperties;
        }

        private AuditPropertyDto GetAuditProperty(string entityName, EntityState state, string fieldName, object fromValue, object toValue)
        {
            if (CompareValues(fromValue, toValue))
                return null;

            //entityName = entityName.ToLowerInvariant();

            //if (IgnoredFields.ContainsKey(entityName))
            //{
            //    var ignoredFields = IgnoredFields[entityName];
            //    var fn = fieldName.ToLower();
            //    while (fn.StartsWith("."))
            //        fn = fn.Substring(1);
            //    if (ignoredFields.Any(p => p == fn))
            //    {
            //        return null;
            //    }
            //}

            //if (MaskedFields.ContainsKey(en))
            //{
            //    var maskedFields = MaskedFields[en];
            //    var fn = fieldName.ToLower();
            //    while (fn.StartsWith("."))
            //        fn = fn.Substring(1);
            //    if (maskedFields.Any(p => p == fn))
            //    {
            //        if (fromValue != null)
            //            fromValue = "(hidden)";
            //        if (toValue != null)
            //            toValue = "(hidden)";
            //    }
            //}

            return new AuditPropertyDto(fieldName, fromValue?.ToString(), toValue?.ToString());
        }

        private bool CompareValues(object a, object b)
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
