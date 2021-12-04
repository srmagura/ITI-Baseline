using ITI.DDD.Domain;
using ITI.DDD.Infrastructure.DataContext;

namespace ITI.DDD.Infrastructure.DataMapping
{
    public interface IDbEntityMapper
    {
        TDb ToDb<TDb>(Entity entity) where TDb : DbEntity;
        TEntity ToEntity<TEntity>(DbEntity dbEntity) where TEntity : Entity;
    }
}
