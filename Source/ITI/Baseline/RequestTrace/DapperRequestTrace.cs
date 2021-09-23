using System;
using System.Data.SqlClient;
using Dapper;
using ITI.DDD.Logging;

namespace ITI.Baseline.RequestTrace
{
    public class DapperRequestTrace : IRequestTrace
    {
        private readonly ILogger _logger;
        private readonly IDbRequestTraceSettings _settings;

        public DapperRequestTrace(ILogger logger, IDbRequestTraceSettings settings)
        {
            _logger = logger;
            _settings = settings;
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
                var trace = new RequestTrace.DbRequestTrace(
                    service, 
                    direction, 
                    dateBeginUtc,
                    DateTimeOffset.UtcNow, 
                    url, 
                    request, 
                    response, 
                    exception
                );

                using (var connection = new SqlConnection(_settings.RequestTraceConnectionString))
                {
                    connection.Open();
                    var sqlStatement = $@"
INSERT INTO {_settings.RequestTraceTableName}
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
