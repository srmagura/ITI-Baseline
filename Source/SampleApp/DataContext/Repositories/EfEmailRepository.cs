﻿using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Core.DataContext;
using Iti.Core.DateTime;
using Iti.Core.Repositories;
using Iti.Core.UnitOfWorkBase;
using Iti.Core.UnitOfWorkBase.Interfaces;
using Iti.Email;
using Iti.Identities;
using Microsoft.EntityFrameworkCore;

namespace DataContext.Repositories
{
    public class EfEmailRepository : Repository<SampleDataContext>, IEmailRepository
    {
        public EfEmailRepository(IUnitOfWork uow) : base(uow)
        {
        }

        public void Add(EmailRecord rec)
        {
            Context.EmailRecords.Add(DbEntity.ToDb<DbEmailRecord>(rec));
        }

        public EmailRecord Get(EmailRecordId id)
        {
            return Context.EmailRecords
                .FirstOrDefault(p => p.Id == id.Guid)
                ?.ToEntity<EmailRecord>();
        }
    }
}