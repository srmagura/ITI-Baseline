using AutoMapper;
using ITI.Baseline.Audit;
using ITI.Baseline.RequestTracing;
using ITI.DDD.Core;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using TestApp.DataContext.DataModel;

namespace TestApp.DataContext;

public class AppDataContext : BaseDataContext, IDataContext, IAuditDataContext
{
    private readonly string _connectionString;

    public AppDataContext()
    {
        _connectionString = new ConnectionStrings().AppDataContext;
    }

    public AppDataContext(IMapper mapper, IAuditor auditor) : base(mapper, auditor)
    {
        _connectionString = new ConnectionStrings().AppDataContext;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .EnableSensitiveDataLogging()
            .UseSqlServer(_connectionString);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        LogEntry.OnModelCreating(modelBuilder);
        RequestTrace.OnModelCreating(modelBuilder);
        AuditRecord.OnModelCreating(modelBuilder);

        DbUser.OnModelCreating(modelBuilder);
        DbLtcPharmacy.OnModelCreating(modelBuilder);
    }

    public DbSet<DbCustomer> Customers => Set<DbCustomer>();
    public DbSet<DbLtcPharmacy> LtcPharmacies => Set<DbLtcPharmacy>();
    public DbSet<DbFacility> Facilities => Set<DbFacility>();

    public DbSet<DbUser> Users => Set<DbUser>();
    public DbSet<DbCustomerUser> CustomerUsers => Set<DbCustomerUser>();
    public DbSet<DbOnCallUser> OnCallUsers => Set<DbOnCallUser>();

    public DbSet<AuditRecord> AuditRecords => Set<AuditRecord>();
    public DbSet<RequestTrace> RequestTraces => Set<RequestTrace>();
    public DbSet<LogEntry> LogEntries => Set<LogEntry>();
}
