using System;
using System.Data.SqlClient;
using Dapper;

namespace Iti.Baseline.Logging
{
    public class DbLogWriter : ILogWriter
    {
        private readonly DbLoggerSettings _settings;

        public DbLogWriter(DbLoggerSettings settings)
        {
            _settings = settings;
        }

        public void Write(string level, string userId, string userName, string hostname, string process, string thread, string message, Exception exc = null)
        {
            try
            {
                using (var conn = new SqlConnection(_settings.ConnectionString))
                {
                    var sql = $@"INSERT INTO {_settings.TableName} 
                                    (WhenUtc,Level,UserId,UserName,Hostname,Process,Thread,Message,Exception) 
                                    VALUES (@WhenUtc,@Level,@UserId,@UserName,@Hostname,@Process,@Thread,@Message,@Exception)";

                    var logEntry = new LogEntry(level, userId, userName, hostname, process, thread, message, exc);

                    conn.Execute(sql, logEntry, commandTimeout: 30);
                }
            }
            catch (Exception logException)
            {
                // eat it... nothing we can do, and don't want to fail anything because of logging issues
                // ... so we send the message screaming into the void...

                // last ditch attempt...
                Console.WriteLine($"DBLOGGER INTERNAL EXCEPTION: {logException}");
            }
        }
    }
}