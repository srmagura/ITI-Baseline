using AutoMapper;
using ITI.DDD.Core;
using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITI.DDD.Infrastructure.DataContext
{
    public abstract class DbEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        [NotMapped]
        [IgnoreMap]
        public Entity? MappedEntity { get; set; }

        public DateTimeOffset DateCreatedUtc { get; set; } = DateTimeOffset.UtcNow;

        [NotMapped]
        public List<IDomainEvent> DomainEvents { get; set; } = new List<IDomainEvent>();
    }
}
