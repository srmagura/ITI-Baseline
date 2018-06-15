using System;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Iti.Core.Sequences
{
    public class EfSequenceResolver<TDbContext> : ISequenceResolver
        where TDbContext : DbContext
    {
        public long GetNextValue(string name)
        {
            var db = UnitOfWork.UnitOfWork.Current<TDbContext>();

            var sql = $"SELECT NEXT VALUE FOR [{Sequence.SequenceSchema}].[{name}]";

            using (var cmd = db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                db.Database.OpenConnection();

                using (var result = cmd.ExecuteReader())
                {
                    result.Read();
                    var value = (long)result[0];
                    // Console.WriteLine($"SEQUENCE: [{value}]");
                    return value;
                }
            }
        }
    }
}