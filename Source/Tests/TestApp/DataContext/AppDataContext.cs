﻿using ITI.Baseline.Audit;
using ITI.DDD.Application;
using ITI.DDD.Core;
using ITI.DDD.Core.Util;
using ITI.DDD.Domain.DomainEvents;
using ITI.DDD.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using RequestTrace;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TestApp.DataContext.DataModel;

namespace TestApp.DataContext
{
    public class AppDataContext : BaseDataContext, IDataContext, IAuditDataContext
    {
        public DbSet<DbCustomer>? Customers { get; set; }
        public DbSet<DbLtcPharmacy>? LtcPharmacies { get; set; }
        public DbSet<AuditRecord>? AuditRecords { get; set; }

        public DbSet<DbRequestTrace>? IncomingGoogleRequests { get; set; }
        public DbSet<DbRequestTrace>? OutgoingGoogleRequests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = GetConnectionString();

            optionsBuilder
                .EnableSensitiveDataLogging()
                .UseSqlServer(connString);

            base.OnConfiguring(optionsBuilder);
        }

        private static ConnectionStrings? _connStrings = null;
        private static readonly object LockObject = new object();
        private static string GetConnectionString()
        {
            if (_connStrings == null)
            {
                lock (LockObject)
                {
                    if (_connStrings == null)
                    {
                        if(IOC.IsStaticInitialized)
                            _connStrings = IOC.ResolveStaticUseSparingly<ConnectionStrings>();
                    }
                }
            }

            return (_connStrings ?? new ConnectionStrings()).DefaultDataContext;
        }

        public static void Migrate()
        {
            using (var context = new AppDataContext())
            {
                context.Database.SetCommandTimeout(600);
                context.Database.Migrate();
            }
        }
    }
}
