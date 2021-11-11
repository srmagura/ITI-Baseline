using AutoMapper;
using ITI.DDD.Domain.Entities;
using ITI.DDD.Infrastructure.DataContext;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public class DbEntityMapper : IDbEntityMapper
    {
        private readonly IMapper _mapper;

        public DbEntityMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        // This always creates a new instance of the DbEntity, even if a DbEntity
        // with this ID is already being tracked
        public TDb ToDb<TDb>(Entity entity) where TDb : DbEntity
        {
            return _mapper.Map<TDb>(entity);
        }

        public TEntity ToEntity<TEntity>(DbEntity dbEntity) where TEntity : Entity
        {
            if (dbEntity == null)
                return null;

            var entity = _mapper.Map<TEntity>(dbEntity);
            dbEntity.MappedEntity = entity;

            return entity;
        }
    }
}
