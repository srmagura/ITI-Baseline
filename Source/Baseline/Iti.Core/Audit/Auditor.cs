using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Auth;
using Iti.Core.ValueObjects;
using Iti.Logging;
using Iti.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Iti.Core.Audit
{
    //
    // NOTE: This is not yet completely tested.
    //       Specifically, nested objects (value objects, collections, etc.) need to be more
    //       thoroughly tested to make sure all changes are captured correctly.
    //

    public class Auditor
    {
        private readonly ILogger _logger;
        private readonly IAuthContext _auth;
        private static readonly Dictionary<string, List<string>> MaskedFields = new Dictionary<string, List<string>>();

        private static readonly Dictionary<string, List<string>> IgnoredFields = new Dictionary<string, List<string>>();

        public static void IgnoreField(string entityName, string fieldName)
        {
            if (!entityName.HasValue() || !fieldName.HasValue())
                return;

            entityName = entityName.ToLower();
            fieldName = fieldName.ToLower();

            if (!IgnoredFields.ContainsKey(entityName))
            {
                IgnoredFields[entityName] = new List<string>();
            }

            if (!IgnoredFields[entityName].Contains(fieldName))
                IgnoredFields[entityName].Add(fieldName);
        }

        public static void MaskField(string entityName, string fieldName)
        {
            if (!entityName.HasValue() || !fieldName.HasValue())
                return;

            entityName = entityName.ToLower();
            fieldName = fieldName.ToLower();

            if (!MaskedFields.ContainsKey(entityName))
            {
                MaskedFields[entityName] = new List<string>();
            }

            if (!MaskedFields[entityName].Contains(fieldName))
                MaskedFields[entityName].Add(fieldName);
        }

        //
        //

        public Auditor(ILogger logger, IAuthContext auth)
        {
            _logger = logger;
            _auth = auth;
        }

        internal void Process(IAuditDataContext db, ChangeTracker changeTracker)
        {
            if (db == null)
                return;

            try
            {
                var changes = BuildAuditRecords(changeTracker, _logger, _auth);

                if (changes.HasItems())
                {
                    db.AuditEntries.AddRange(changes);
                }
            }
            catch (Exception exc)
            {
                _logger?.Error("Could not process audit", exc);
                // eat exception!
            }
        }

        private List<AuditRecord> BuildAuditRecords(ChangeTracker changeTracker, ILogger logger, IAuthContext auth)
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

                        if (entry.State == EntityState.Unchanged && !HasReferenceChanges(entry))
                            continue;

                        var changeType = entry.State.ToString();
                        var changes = GetChangeDetails(auditEntity.AuditEntityName, entry);

                        var aggregateName = auditEntity.AuditEntityName;
                        var aggregateId = auditEntity.AuditEntityId;

                        if (entity is IDbAuditedChild child)
                        {
                            aggregateId = child.AuditAggregateId;
                            aggregateName = child.AuditAggregateName;

                            if (!child.HasParent)
                                changeType = "Deleted";
                        }

                        if (ShouldAudit(changes))
                        {
                            var audit = new AuditRecord(auth?.UserId, auth?.UserName,
                                aggregateName, aggregateId,
                                auditEntity.AuditEntityName, auditEntity.AuditEntityId,
                                changeType, changes);

                            list.Add(audit);
                        }
                    }
                }
                catch (Exception exc)
                {
                    logger?.Error("Audit Error", exc);
                }
            }

            return list;
        }

        private bool ShouldAudit(string changes)
        {
            return changes.HasValue() && changes != "[]";
        }

        private bool HasReferenceChanges(EntityEntry entry)
        {
            return entry?.References.Any(HasChanges) ?? false;
        }

        private bool HasChanges(ReferenceEntry entry)
        {
            if (entry?.TargetEntry == null)
                return false;

            return entry.TargetEntry.State == EntityState.Added
                   || entry.TargetEntry.State == EntityState.Modified
                   || entry.TargetEntry.State == EntityState.Deleted;
        }

        private string GetChangeDetails(string entityName, EntityEntry entry)
        {
            var state = entry.State;

            if (entry.Entity is IDbAuditedChild ch)
            {
                if (!ch.HasParent)
                    state = EntityState.Deleted;
            }

            var auditProperties = new List<AuditProperty>();

            //

            var fromValues = entry.OriginalValues;
            var toValues = entry.CurrentValues;

            if (state == EntityState.Added || state == EntityState.Deleted)
                toValues = null;

            AddNestedFields(entityName, auditProperties, state, "", fromValues, toValues);

            //

            foreach (var rf in entry.References)
            {
                if (rf?.TargetEntry?.Entity is IValueObject)
                {
                    AddValueObject(entityName, rf, auditProperties);
                }
            }

            var auditResult = auditProperties.ToJson(Formatting.None);

            return auditResult;
        }

        private void AddValueObject(string entityName, ReferenceEntry rf, List<AuditProperty> auditProperties)
        {
            var state = rf.TargetEntry.State;

            var fromValues = rf.TargetEntry.OriginalValues;
            var toValues = rf.TargetEntry.CurrentValues;

            if (state == EntityState.Added || state == EntityState.Deleted)
                toValues = null;

            AddNestedFields(entityName, auditProperties, state, rf.Metadata.Name, fromValues, toValues);
        }

        private void AddNestedFields(string entityName, List<AuditProperty> auditProperties, EntityState state, string prefix,
            PropertyValues fromValues, PropertyValues toValues)
        {
            foreach (var prop in fromValues.Properties)
            {
                if (prop.IsShadowProperty)
                    continue;

                var fieldName = prop.Name;

                if (fieldName == "HasValue" || fieldName.EndsWith("BackingField"))
                    continue;

                var fromValue = fromValues[fieldName];
                var toValue = toValues?[fieldName];

                if (fromValue is PropertyValues)
                {
                    // TODO: don't think this ever gets called with EFCore2
                    AddNestedFields(entityName, auditProperties, state, $"{prefix}.{fieldName}",
                        fromValue as PropertyValues, toValue as PropertyValues);
                }
                else
                {
                    var changeInfo = AddField(entityName, state, $"{prefix}.{fieldName}", fromValue, toValue);
                    if (changeInfo != null)
                        auditProperties.Add(changeInfo);
                }
            }
        }

        private bool CompareValues(object a, object b)
        {
            try
            {
                if (a == null && b == null)
                    return true;

                if (a == null || b == null)
                    return false;

                if (a is decimal d1 && b is decimal d2)
                {
                    return d1 == d2;
                }
            }
            catch
            {
                // ignore and default to string comparison
            }

            return a.ToString() == b.ToString();
        }

        private AuditProperty AddField(string entityName, EntityState state, string fieldName, object fromValue, object toValue)
        {
            var areEqual = CompareValues(fromValue, toValue);
            
            if (areEqual)
                return null;

            if (state == EntityState.Added)
            {
                var temp = fromValue;
                fromValue = toValue;
                toValue = temp;
            }

            var en = entityName.ToLower();

            if (IgnoredFields.ContainsKey(en))
            {
                var ignoredFields = IgnoredFields[en];
                var fn = fieldName.ToLower();
                while (fn.StartsWith("."))
                    fn = fn.Substring(1);
                if (ignoredFields.Any(p => p == fn))
                {
                    return null;
                }
            }

            if (MaskedFields.ContainsKey(en))
            {
                var maskedFields = MaskedFields[en];
                var fn = fieldName.ToLower();
                while (fn.StartsWith("."))
                    fn = fn.Substring(1);
                if (maskedFields.Any(p => p == fn))
                {
                    if (fromValue != null)
                        fromValue = "(hidden)";
                    if (toValue != null)
                        toValue = "(hidden)";
                }
            }

            return new AuditProperty(fieldName, fromValue?.ToString(), toValue?.ToString());
        }
    }
}
