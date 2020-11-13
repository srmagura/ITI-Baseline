using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;
using ITI.DDD.Application.UnitOfWork;
using ITI.DDD.Infrastructure;
using ITI.DDD.Infrastructure.DataMapping;
using ITI.DDD.Logging;
using RequestTrace;
using TestApp.DataContext;

namespace TestApp.Infrastructure
{
    public class DapperRequestTrace : IRequestTrace
    {
        private readonly ILogger _logger;
        private readonly ConnectionStrings _connectionStrings;

        public DapperRequestTrace(ILogger logger, ConnectionStrings connectionStrings)
        {
            _logger = logger;
            _connectionStrings = connectionStrings;
        }

        public void WriteTrace(
            string service, 
            RequestTraceDirection direction,
            DateTimeOffset dateBeginUtc,
            string url,
            string request,
            string response,
            Exception? exception
        )
        {
            try
            {
                var trace = new DbRequestTrace(
                    service, 
                    direction, 
                    dateBeginUtc, 
                    DateTimeOffset.UtcNow, 
                    url, 
                    request, 
                    response, 
                    exception
                );

                using (var connection = new SqlConnection(_connectionStrings.DefaultDataContext))
                {
                    connection.Open();
                    var sqlStatement = @"
INSERT INTO RequestTraces
VALUES (
@Service,
@Direction,
@DateBeginUtc,
@DateEndUtc,
@Url,
@Request,
@Response,
@Exception)";
                    connection.Execute(sqlStatement, trace);
                }
            }
            catch (Exception exc)
            {
                _logger.Error("Could not create request trace", exc);
            }
        }
    }
}
