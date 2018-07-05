using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iti.Core.Audit;
using Iti.Core.DataContext;
using Iti.ValueObjects;

namespace DataContext
{
    public class DbFoo : DbEntity, IDbAudited
    {
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string NotInEntity { get; set; }

        //public PersonName PersonName { get; set; }
        //public PhoneNumber PhoneNumber { get; set; }
        public Address Address { get; set; }

        public List<DbBar> Bars { get; set; }

        public string SomeInts { get; set; }

        [Column(TypeName="Money")]
        public decimal SomeMoney { get; set; }

        public string SomeGuids { get; set; }

        public long SomeNumber { get; set; }

        // AUDIT

        public string AuditEntityName => "Foo";
        public string AuditEntityId => Id.ToString();
    }
}