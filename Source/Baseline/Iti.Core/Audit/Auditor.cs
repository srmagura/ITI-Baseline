using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Auth;
using Iti.Core.ValueObjects;
using Iti.Inversion;
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

    public static class Auditor
    {
        internal static void Process(IAuditDataContext db, ChangeTracker changeTracker)
        {
            if (db == null)
                return;

            try
            {
                var changes = BuildAuditRecords(changeTracker);

                if (changes.HasItems())
                {
                    db.AuditEntries.AddRange(changes);
                }
            }
            catch (Exception exc)
            {
                Log.Error("Could not process audit", exc);
                // eat exception!
            }
        }

        private static List<AuditRecord> BuildAuditRecords(ChangeTracker changeTracker)
        {
            var auth = IOC.TryResolve<IAuthContext>();

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
                        var changes = GetChangeDetails(entry);

                        var aggregateName = auditEntity.AuditEntityName;
                        var aggregateId = auditEntity.AuditEntityId;

                        if (entity is IDbAuditedChild child)
                        {
                            aggregateId = child.AuditAggregateId;
                            aggregateName = child.AuditAggregateName;

                            if (!child.HasParent)
                                changeType = "Deleted";
                        }

                        var audit = new AuditRecord(auth?.UserId, auth?.UserName,
                            aggregateName, aggregateId,
                            auditEntity.AuditEntityName, auditEntity.AuditEntityId,
                            changeType, changes);

                        list.Add(audit);
                    }
                }
                catch (Exception exc)
                {
                    Log.Error("Audit Error", exc);
                }
            }

            return list;
        }

        private static bool HasReferenceChanges(EntityEntry entry)
        {
            return entry.References.Any(HasChanges);
        }

        private static bool HasChanges(ReferenceEntry entry)
        {
            return entry.TargetEntry.State == EntityState.Added
                   || entry.TargetEntry.State == EntityState.Modified
                   || entry.TargetEntry.State == EntityState.Deleted;
        }

        private static string GetChangeDetails(EntityEntry entry)
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

            AddNestedFields(auditProperties, state, "", fromValues, toValues);

            //

            foreach (var rf in entry.References)
            {
                if (rf?.TargetEntry?.Entity is IValueObject vobj)
                {
                    AddValueObject(rf, auditProperties);
                }
            }

            var auditResult = auditProperties.ToJson(Formatting.None);

            return auditResult;
        }

        private static void AddValueObject(ReferenceEntry rf, List<AuditProperty> auditProperties)
        {
            var state = rf.TargetEntry.State;

            var fromValues = rf.TargetEntry.OriginalValues;
            var toValues = rf.TargetEntry.CurrentValues;

            if (state == EntityState.Added || state == EntityState.Deleted)
                toValues = null;

            AddNestedFields(auditProperties, state, rf.Metadata.Name, fromValues, toValues);
        }

        private static void AddNestedFields(List<AuditProperty> auditProperties, EntityState state, string prefix, 
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
                    AddNestedFields(auditProperties, state, $"{prefix}.{fieldName}", 
                        fromValue as PropertyValues, toValue as PropertyValues);
                }
                else
                {
                    var changeInfo = AddField(state, $"{prefix}.{fieldName}", fromValue, toValue);
                    if (changeInfo != null)
                        auditProperties.Add(changeInfo);
                }
            }
        }

        private static AuditProperty AddField(EntityState state, string fieldName, object fromValue, object toValue)
        {
            if (fromValue == null && toValue == null)
                return null;

            if (fromValue?.ToString() == toValue?.ToString())
                return null;

            if (state == EntityState.Added)
            {
                var temp = fromValue;
                fromValue = toValue;
                toValue = temp;
            }

            return new AuditProperty(fieldName, fromValue?.ToString(), toValue?.ToString());
        }
    }
}
