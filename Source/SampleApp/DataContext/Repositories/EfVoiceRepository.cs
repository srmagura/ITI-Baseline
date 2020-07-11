using System.Linq;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.Repositories;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Identities;
using Iti.Baseline.Voice;

namespace DataContext.Repositories
{
    public class EfVoiceRepository : Repository<SampleDataContext>, IVoiceRepository
    {
        public EfVoiceRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public void Add(VoiceRecord rec)
        {
            Context.VoiceRecords.Add(DbEntity.ToDb<DbVoiceRecord>(rec));
        }

        public VoiceRecord Get(VoiceRecordId id)
        {
            return Context.VoiceRecords
                .FirstOrDefault(p => p.Id == id.Guid)
                ?.ToEntity<VoiceRecord>();
        }
    }
}