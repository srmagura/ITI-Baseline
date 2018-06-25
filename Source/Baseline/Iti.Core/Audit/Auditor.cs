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

            foreach (var rf in entry.References)
            {
                if (rf?.TargetEntry?.Entity is IValueObject vobj)
                {
                    // Console.WriteLine($"****** REF: {rf.Metadata.Name} / {rf.Metadata.PropertyInfo.Name}");

                    var rfstate = rf.TargetEntry.State;

                    var rfOrigValues = state == EntityState.Modified ? rf.TargetEntry.OriginalValues : null;
                    var rfCurrentValues = state == EntityState.Deleted ? rf.TargetEntry.OriginalValues : rf.TargetEntry.CurrentValues;

                    AddNestedFields(auditProperties, rfstate, rf.Metadata.Name, rfCurrentValues, rfOrigValues);
                }
            }

            var auditResult = auditProperties.ToJson(Formatting.None);

            /*
            if (entry.Entity.GetType().Name == "DbFoo")
            {
                Console.WriteLine("((((((((((((((((((((((((((((");
                Console.WriteLine("AUDIT");
                Console.WriteLine(auditResult);
                Console.WriteLine("))))))))))))))))))))))))))))");
            }
            */

            return auditResult;
        }

        private static void AddNestedFields(List<AuditProperty> auditProperties, EntityState state, string prefix, PropertyValues currentValues, PropertyValues originalValues)
        {
            foreach (var prop in currentValues.Properties)
            {
                if (prop.IsShadowProperty)
                    continue;

                var fieldName = prop.Name;

                if (fieldName == "HasValue" || fieldName.EndsWith("BackingField"))
                    continue;

                var currValue = currentValues[fieldName];
                var origValue = originalValues?[fieldName];

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
            if (currValue == null && origValue == null)
                return null;

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
