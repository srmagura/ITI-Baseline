using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Auth;
using Iti.Inversion;
using Iti.Logging;
using Iti.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Iti.Core.Audit
{
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
                        if (entry.State == EntityState.Unchanged
                            || entry.State == EntityState.Detached)
                            continue;

                        var changeType = entry.State.ToString();
                        var changes = GetChangeDetails(entry);

                        var aggregateName = auditEntity.AuditEntityName;
                        var aggregateId = auditEntity.AuditEntityId;

                        if (entity is IDbAuditedChild child)
                        {
                            aggregateId = child.AuditAggregateId;
                            aggregateName = child.AuditAggregateName;
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

        private static string GetChangeDetails(EntityEntry entry)
        {
            var state = entry.State;

            var auditProperties = new List<AuditProperty>();

            var origValues = state == EntityState.Modified ? entry.OriginalValues : null;
            var currentValues = state == EntityState.Deleted ? entry.OriginalValues : entry.CurrentValues;
            AddNestedFields(auditProperties, state, "", currentValues, origValues);

            return auditProperties.ToJson(Formatting.None);
        }

        private static void AddNestedFields(List<AuditProperty> auditProperties, EntityState state, string prefix, PropertyValues currentValues, PropertyValues originValues)
        {
            foreach (var prop in currentValues.Properties)
            {
                var fieldName = prop.Name;

                if (fieldName == "HasValue" || fieldName.EndsWith("BackingField"))
                    continue;

                var currValue = currentValues[fieldName];
                var origValue = originValues?[fieldName];

                if (currValue is PropertyValues)
                {
                    AddNestedFields(auditProperties, state, $"{prefix}.{fieldName}", currValue as PropertyValues, origValue as PropertyValues);
                }
                else
                {
                    var changeInfo = AddField(state, $"{prefix}.{fieldName}", currValue, origValue);
                    if (changeInfo != null)
                        auditProperties.Add(changeInfo);
                }
            }
        }

        private static AuditProperty AddField(EntityState state, string fieldName, object currValue, object origValue)
        {
            var includeField = true;
            if (state == EntityState.Modified)
                includeField = (currValue?.ToString() != origValue?.ToString());

            return includeField
                    ? new AuditProperty(fieldName, origValue?.ToString(), currValue?.ToString())
                    : null
                ;
        }
    }
}
