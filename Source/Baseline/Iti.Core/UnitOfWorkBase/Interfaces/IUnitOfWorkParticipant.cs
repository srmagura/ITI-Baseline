using Iti.Baseline.Core.Audit;
using Iti.Baseline.Core.DomainEventsBase;

namespace Iti.Baseline.Core.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWorkParticipant
    {
        void OnUnitOfWorkCommit(Auditor auditor, DomainEvents domainEvents);
    }
}