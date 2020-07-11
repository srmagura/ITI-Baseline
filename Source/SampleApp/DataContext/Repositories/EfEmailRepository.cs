using System;
using System.Collections.Generic;
using System.Linq;
using Iti.Baseline.Core.DataContext;
using Iti.Baseline.Core.Repositories;
using Iti.Baseline.Core.UnitOfWorkBase.Interfaces;
using Iti.Baseline.Email;
using Iti.Baseline.Identities;
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