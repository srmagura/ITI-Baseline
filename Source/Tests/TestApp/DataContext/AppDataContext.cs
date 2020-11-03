using ITI.DDD.Core;
using ITI.DDD.Core.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestApp.DataContext
{
    public class AppDataContext : DbContext
    {
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
                        _connStrings = IOC.ResolveStaticUseSparingly<ConnectionStrings>();
                    }
                }
            }

            return _connStrings.DefaultDataContext;
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
