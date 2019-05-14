using System;
using System.Collections.Generic;
using Iti.Identities;

namespace Iti.Email
{
    public interface IEmailRepository
    {
        void Add(EmailRecord rec);
        EmailRecord Get(EmailRecordId id);
    }
}