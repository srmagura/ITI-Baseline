using System;
using Iti.Auth;
using Iti.Inversion;

namespace Iti.Core.Audit
{
    public static class AuditEvents
    {
        public static void Write(string entity, string entityId, string text)
        {
            var auth = IOC.TryResolve<IAuthContext>();

            var rec = new AuditRecord(auth?.UserId, auth?.UserName,
                entity, entityId,
                entity, entityId,
                "Event", text);

            using (var db = IOC.TryResolve<IAuditDataContext>())
            {
                if (db == null)
                    throw new Exception("IAuditDataContext not registered (IOC)");

                db.AuditEntries.Add(rec);

                db.SaveChanges();
            }
        }
    }
}