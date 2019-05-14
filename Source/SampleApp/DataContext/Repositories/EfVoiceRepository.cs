using System.Linq;
using Iti.Core.DataContext;
using Iti.Core.Repositories;
using Iti.Identities;
using Iti.Voice;

namespace DataContext.Repositories
{
    public class EfVoiceRepository : Repository<SampleDataContext>, IVoiceRepository
    {
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