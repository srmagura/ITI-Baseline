using System;
using Iti.Inversion;

namespace Iti.Logging
{
    public class EfLogWriter : ILogWriter
    {
        public void Write(string level, string userId, string userName, string hostname, string process, string thread, string message, Exception exc = null)
        {
            try
            {
                var entry = new LogEntry(level, userId, userName, hostname, process, thread, message, exc);

                using (var db = IOC.TryResolve<ILogDataContext>())
                {
                    if (db == null)
                        return;

                    db.LogEntries.Add(entry);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                // eat it (can't log it, obviously)
            }
        }
    }
}