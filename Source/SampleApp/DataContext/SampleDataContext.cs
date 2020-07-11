using System;
using System.Configuration;
using Autofac;
using Domain;
using Iti.Baseline.Core.Audit;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.Sequences;
using Iti.Baseline.Email;
using Iti.Baseline.Inversion;
using Iti.Baseline.Logging;
using Iti.Baseline.Sms;
using Iti.Baseline.Voice;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataContext
{
    public class SampleDataContext : BaseDataContext, ILogDataContext, IAuditDataContext
    {
        public const string DefaultDatabaseName = "ItiBaselineSample";

        public SampleDataContext() : base() { }

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

        private static ConnectionStrings _connStrings = null;
        private static readonly object LockObject = new object();
        public static string GetConnectionString()
        {
            if (_connStrings == null)
            {
                lock (LockObject)
                {
                    if (_connStrings == null)
                    {
                        IOC.Container.TryResolve(out _connStrings);
                    }
                }
            }

            return _connStrings?.DefaultDataContext
                   ?? ConfigurationManager.ConnectionStrings["DefaultDataContext"]?.ConnectionString
                   ?? $"Server=localhost;Database={DefaultDatabaseName};Trusted_Connection=True;";
        }

        //
        // ENTITIES
        //

        public DbSet<DbFoo> Foos { get; set; }
        public DbSet<DbBar> Bars { get; set; }
        public DbSet<DbValObjHolder> ValObjHolders { get; set; }

        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<AuditRecord> AuditEntries { get; set; }

        public DbSet<DbEmailRecord> EmailRecords { get; set; }
        public DbSet<DbSmsRecord> SmsRecords { get; set; }
        public DbSet<DbVoiceRecord> VoiceRecords { get; set; }
    }
}