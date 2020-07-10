using Iti.Core.Audit;
using Iti.Core.DomainEventsBase;

namespace Iti.Core.UnitOfWorkBase.Interfaces
{
    public interface IUnitOfWorkParticipant
    {
        void OnUnitOfWorkCommit(Auditor auditor, DomainEvents domainEvents);
    }
}