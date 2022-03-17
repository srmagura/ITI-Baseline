using System.ComponentModel.DataAnnotations.Schema;
using ITI.DDD.Core;
using ITI.DDD.Domain;

namespace ITI.DDD.Infrastructure.DataContext
{
    public abstract class DbEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = SequentialGuid.Next();

        [NotMapped]
        public Entity? MappedEntity { get; internal set; }

        public DateTimeOffset DateCreatedUtc { get; set; } = DateTimeOffset.UtcNow;

        [NotMapped]
        public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();
    }
}
