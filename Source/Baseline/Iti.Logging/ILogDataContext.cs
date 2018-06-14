using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Iti.Logging
{
    public interface ILogDataContext : IDisposable
    {
        DbSet<LogEntry> LogEntries { get; set; }
        DatabaseFacade Database { get; }
        int SaveChanges();
    }
}