using ITI.Baseline.Audit;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using ITI.Baseline.RequestTrace;
using TestApp.DataContext.DataModel;
using ITI.DDD.Core;
using AutoMapper;

namespace TestApp.DataContext
{
    public class AppDataContext : BaseDataContext, IDataContext, IAuditDataContext
    {
        public DbSet<DbCustomer> Customers => Set<DbCustomer>();
        public DbSet<DbLtcPharmacy> LtcPharmacies => Set<DbLtcPharmacy>();
        public DbSet<DbFacility> Facilities => Set<DbFacility>();

        public DbSet<DbUser> Users => Set<DbUser>();
        public DbSet<DbCustomerUser> CustomerUsers => Set<DbCustomerUser>();
        public DbSet<DbOnCallUser> OnCallUsers => Set<DbOnCallUser>();

        public DbSet<AuditRecord> AuditRecords => Set<AuditRecord>();
        public DbSet<RequestTrace> RequestTraces => Set<RequestTrace>();
        public DbSet<LogEntry> LogEntries => Set<LogEntry>();

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

        protected override void OnModelCreating(ModelBuilder mb)
        {
            LogEntry.OnModelCreating(mb);
            RequestTrace.OnModelCreating(mb);
            AuditRecord.OnModelCreating(mb);

            DbUser.OnModelCreating(mb);
            DbLtcPharmacy.OnModelCreating(mb);
        }
    }
}
