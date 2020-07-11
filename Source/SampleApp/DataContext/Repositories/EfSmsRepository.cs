using System.Linq;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.Repositories;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Identities;
using Iti.Baseline.Sms;

namespace DataContext.Repositories
{
    public class EfSmsRepository : Repository<SampleDataContext>, ISmsRepository
    {
        public EfSmsRepository(IUnitOfWork uow) : base(uow)
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