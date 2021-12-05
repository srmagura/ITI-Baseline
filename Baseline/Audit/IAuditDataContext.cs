using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ITI.Baseline.Audit;

public interface IAuditDataContext : IDisposable
{
    DbSet<AuditRecord> AuditRecords { get; }
    DatabaseFacade Database { get; }
    int SaveChanges();
}
