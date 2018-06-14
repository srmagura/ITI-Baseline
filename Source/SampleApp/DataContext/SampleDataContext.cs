using System.Configuration;
using Iti.Core.DataContext;
using Iti.Core.Sequences;
using Iti.Email;
using Iti.Inversion;
using Iti.Logging;
using Iti.Sms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataContext
{
    public class SampleDataContext : BaseDataContext, ILogDataContext
    {
        public const string DefaultDatabaseName = "ItiBaselineSample";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = GetConnectionString();

            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(connString, options => options.EnableRetryOnFailure())
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning))
                ;

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Sequence.Initialize(modelBuilder);
            Sequence.Define(modelBuilder, "OrderNumber", 10000, 5);

            base.OnModelCreating(modelBuilder);
        }

        private static string GetConnectionString()
        {
            IOC.TryResolve<ConnectionStrings>(out var connStrings);
            var connString = connStrings != null 
                ? connStrings.DefaultDataContext 
                : ConfigurationManager.ConnectionStrings["DefaultDataContext"]?.ConnectionString;

            return connString 
                   ?? $"Server=localhost;Database={DefaultDatabaseName};Trusted_Connection=True;";
        }

        //
        // ENTITIES
        //

        public DbSet<DbFoo> Foos { get; set; }
        public DbSet<DbBar> Bars { get; set; }

        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<EmailRecord> EmailRecords { get; set; }
        public DbSet<SmsRecord> SmsRecords { get; set; }
    }
}