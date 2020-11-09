using AutoMapper;
using ITI.DDD.Application;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Infrastructure.DataContext
{
    public abstract class BaseDataContext : DbContext, IDataContext
    {
        private IMapper? _mapper;
        private IAuditor? _auditor;

        public void Initialize(IMapper mapper, IAuditor auditor)
        {
            _mapper = mapper;
            _auditor = auditor;
        }

        void IDataContext.SaveChanges()
        {
            SaveChanges();
        }

        public override int SaveChanges()
        {
            UpdateEntityMaps();
            _auditor?.Process(this);

            return base.SaveChanges();
        }

        private void UpdateEntityMaps()
        {
            if(_mapper == null)
                throw new Exception("SaveChanges called before DataContext initialized.");

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is DbEntity dbEntity)
                {
                    if (dbEntity.MappedEntity != null)
                    {
                        _mapper.Map(dbEntity.MappedEntity, dbEntity);
                    }
                }
            }

            ChangeTracker.DetectChanges();
        }
    }
}
