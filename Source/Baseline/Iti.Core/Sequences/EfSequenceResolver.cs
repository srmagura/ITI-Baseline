using System.Data;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Iti.Baseline.Core.Sequences
{
    public class EfSequenceResolver<TDbContext> : ISequenceResolver
        where TDbContext : BaseDataContext, new()
    {
        private readonly IUnitOfWork _uow;

        public EfSequenceResolver(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public long GetNextValue(string name)
        {
            var db = _uow.Current<TDbContext>();

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

                    return value;
                }
            }
        }
    }
}