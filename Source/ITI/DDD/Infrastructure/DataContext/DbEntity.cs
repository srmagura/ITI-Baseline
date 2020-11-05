using AutoMapper;
using ITI.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITI.DDD.Infrastructure.DataContext
{
    public abstract class DbEntity
    {
        public Guid Id { get; set; } = SequentialGuid.Next();
        public DateTimeOffset DateCreatedUtc { get; set; } = DateTimeOffset.UtcNow;

        [NotMapped]
        [IgnoreMap]
        public Entity? MappedEntity { get; set; }
    }
}
