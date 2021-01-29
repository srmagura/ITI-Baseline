using AutoMapper;
using ITI.DDD.Domain.Entities;
using ITI.DDD.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public class DbEntityMapper : IDbEntityMapper
    {
        private readonly IMapper _mapper;

        public DbEntityMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDb ToDb<TDb>(Entity entity) where TDb : DbEntity
        {
            return _mapper.Map<TDb>(entity);
        }

        public TEntity ToEntity<TEntity>(DbEntity dbEntity) where TEntity : Entity
        {
            var entity = _mapper.Map<TEntity>(dbEntity);
            dbEntity.MappedEntity = entity;

            return entity;
        }
    }
}
