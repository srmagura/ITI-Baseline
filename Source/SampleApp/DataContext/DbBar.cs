using System;
using System.ComponentModel.DataAnnotations;
using Iti.Baseline.Core.Audit;
using Iti.Baseline.Core.DataContext;

namespace DataContext
{
    public class DbBar : DbEntity, IDbAuditedChild
    {
        public Guid FooId { get; set; }
        public DbFoo Foo { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string NotInEntity { get; set; }

        // AUDIT

        public string AuditEntityName => "Bar";
        public string AuditEntityId => Id.ToString();
        public string AuditAggregateName => "Foo";
        public string AuditAggregateId => FooId.ToString();

        public bool HasParent => Foo != null;
    }
}