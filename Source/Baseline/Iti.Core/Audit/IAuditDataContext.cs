using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Iti.Core.Audit
{
    public interface IAuditDataContext : IDisposable
    {
        DbSet<AuditRecord> AuditEntries { get; }
        DatabaseFacade Database { get; }
        int SaveChanges();
    }
}