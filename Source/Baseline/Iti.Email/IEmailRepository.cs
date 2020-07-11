using Iti.Baseline.Identities;

namespace Iti.Baseline.Email
{
    public interface IEmailRepository
    {
        void Add(EmailRecord rec);
        EmailRecord Get(EmailRecordId id);
    }
}