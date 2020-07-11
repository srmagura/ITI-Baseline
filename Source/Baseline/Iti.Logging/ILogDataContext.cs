using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Iti.Baseline.Logging
{
    public interface ILogDataContext : IDisposable
    {
        DbSet<LogEntry> LogEntries { get; }
        DatabaseFacade Database { get; }
        int SaveChanges();
    }
}