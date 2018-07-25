using System;
using Iti.Core.DateTime;
using Iti.Core.Services;
using Iti.Inversion;
using Microsoft.EntityFrameworkCore;

namespace Iti.Logging.Job
{
    public class LogCleanupJobProcessor : JobProcessor
    {
        private readonly LogCleanupSettings _settings;

        public LogCleanupJobProcessor(LogCleanupSettings settings)
        {
            _settings = settings;
        }

        public override void Run()
        {
            try
            {
                using (var db = IOC.TryResolve<ILogDataContext>())
                {
                    if (db == null)
                        return;

                    var dt = DateTimeService.UtcNow.AddDays(-1 * _settings.LogLifetimeDays);

                    db.Database.ExecuteSqlCommand("DELETE FROM LogEntries WHERE WhenUtc < {0}", dt);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
    }
}