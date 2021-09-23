using ITI.Baseline.Audit;
using ITI.DDD.Application;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using ITI.Baseline.RequestTrace;
using TestApp.DataContext.DataModel;
using TestApp.Domain.Enums;

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
        public DbSet<DbRequestTrace> RequestTraces => Set<DbRequestTrace>();
        public DbSet<LogEntry> LogEntries => Set<LogEntry>();

        private readonly string _connectionString;

        public AppDataContext(ConnectionStrings connectionStrings)
        {
            _connectionString = connectionStrings.DefaultDataContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        public static void Migrate(ConnectionStrings connectionStrings)
        {
            using (var context = new AppDataContext(connectionStrings))
            {
                context.Database.SetCommandTimeout(600);
                context.Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbRequestTrace>()
                .HasIndex(t => new { t.Service, t.Direction });

            modelBuilder.Entity<DbUser>()
                .HasDiscriminator(u => u.Type)
                .HasValue<DbCustomerUser>(UserType.Customer)
                .HasValue<DbOnCallUser>(UserType.OnCall);

            modelBuilder.Entity<LogEntry>().HasIndex(p => p.WhenUtc);
        }
    }
}
