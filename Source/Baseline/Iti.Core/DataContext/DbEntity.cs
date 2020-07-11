using System;
using System.ComponentModel.DataAnnotations.Schema;
using AutoMapper;
using Iti.Core.Entites;
using Iti.Core.Mapping;
using Newtonsoft.Json;

namespace Iti.Core.DataContext
{
    public abstract class DbEntity
    {
        public Guid Id { get; set; } = SequentialGuid.Next();
        public DateTimeOffset DateCreatedUtc { get; set; } = DateTimeOffset.UtcNow;

        [JsonIgnore]
        [NotMapped]
        [IgnoreMap]
        public Entity MappedEntity { get; set; }

        public static TDb ToDb<TDb>(Entity e)
            where TDb : DbEntity
        {
            var dbe = Mapper.Map<TDb>(e);

            // BaseDataMapConfig.FillNullValueObjects(dbe);

            if (dbe != null)
            {
                dbe.MappedEntity = e;
            }

            return dbe;
        }

        public TEntity ToEntity<TEntity>()
            where TEntity : Entity
        {
            if (MappedEntity != null)
                return (TEntity)MappedEntity;

            var e = Mapper.Map<TEntity>(this);

            BaseDataMapConfig.RemoveEmptyValueObjects(e);

            MappedEntity = e;

            return e;
        }
    }
}