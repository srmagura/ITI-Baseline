using ITI.Baseline.Audit;
using ITI.DDD.Application;
using ITI.DDD.Core;
using ITI.DDD.Core.Util;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Infrastructure.DataContext;
using ITI.DDD.Logging;
using Microsoft.EntityFrameworkCore;
using RequestTrace;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TestApp.DataContext.DataModel;
using TestApp.Domain.Enums;

namespace TestApp.DataContext
{
    public class AppDataContext : BaseDataContext, IDataContext, IAuditDataContext
    {
        public DbSet<DbCustomer>? Customers { get; set; }
        public DbSet<DbLtcPharmacy>? LtcPharmacies { get; set; }
        public DbSet<DbFacility>? Facilities { get; set; }
        
        public DbSet<DbUser>? Users { get; set; }
        public DbSet<DbCustomerUser>? CustomerUsers { get; set; }
        public DbSet<DbOnCallUser>? OnCallUsers { get; set; }

        public DbSet<AuditRecord>? AuditRecords { get; set; }
        public DbSet<DbRequestTrace>? RequestTraces { get; set; }
        public DbSet<LogEntry>? LogEntries { get; set; }

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
