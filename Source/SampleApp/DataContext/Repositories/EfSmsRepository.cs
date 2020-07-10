using System.Linq;
using Iti.Core.DataContext;
using Iti.Core.Repositories;
using Iti.Core.UnitOfWorkBase;
using Iti.Identities;
using Iti.Sms;

namespace DataContext.Repositories
{
    public class EfSmsRepository : Repository<SampleDataContext>, ISmsRepository
    {
        public EfSmsRepository(UnitOfWork uow) : base(uow)
        {
        }

        public void Add(SmsRecord rec)
        {
            Context.SmsRecords.Add(DbEntity.ToDb<DbSmsRecord>(rec));
        }

        public SmsRecord Get(SmsRecordId id)
        {
            return Context.SmsRecords
                .FirstOrDefault(p => p.Id == id.Guid)
                ?.ToEntity<SmsRecord>();
        }
    }
}