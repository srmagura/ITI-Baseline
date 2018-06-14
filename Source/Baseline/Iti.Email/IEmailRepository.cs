using System;
using System.Collections.Generic;

namespace Iti.Email
{
    public interface IEmailRepository
    {
        void Add(EmailRecord rec);
        EmailRecord Get(long id);

        void ForEachPendingOrRetry(Action<EmailRecord> callback);
        void CleanupOldEmails(int olderThanDays);
    }
}