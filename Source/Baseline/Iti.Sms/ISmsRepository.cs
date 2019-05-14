using System;
using System.Collections.Generic;
using Iti.Identities;

namespace Iti.Sms
{
    public interface ISmsRepository
    {
        void Add(SmsRecord rec);
        SmsRecord Get(SmsRecordId id);
    }
}