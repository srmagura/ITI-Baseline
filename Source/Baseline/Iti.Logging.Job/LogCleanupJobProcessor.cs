using System;
using Iti.Core.DateTime;
using Iti.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace Iti.Logging.Job
{
    public class LogCleanupJobProcessor : JobProcessor
    {
        private readonly ILogDataContext _context;
        private readonly LogCleanupSettings _settings;

        public LogCleanupJobProcessor(ILogger logger, ILogDataContext context, LogCleanupSettings settings)
            : base(logger)
        {
            _context = context;
            _settings = settings;
        }

        public override void Run()
        {
            try
            {
                if (_context == null)
                    return;

                var dt = DateTimeService.UtcNow.AddDays(-1 * _settings.LogLifetimeDays);

                _context.Database.ExecuteSqlCommand("DELETE FROM LogEntries WHERE WhenUtc < {0}", dt);
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
    }
}