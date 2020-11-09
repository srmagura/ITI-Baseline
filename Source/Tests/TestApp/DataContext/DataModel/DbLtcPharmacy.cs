using ITI.Baseline.Audit;
using ITI.DDD.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestApp.DataContext.DataModel
{
    public class DbLtcPharmacy : DbEntity, IDbAuditedChild
    {
        [MaxLength(64)]
        public string? Name { get; set; }

        public DbCustomer? Customer { get; set; }
        public Guid CustomerId { get; set; }

        public string AuditAggregateName => "Customer";
        public string AuditAggregateId => CustomerId.ToString();
        public bool HasParent => true;
        
        public string AuditEntityName => "LtcPharmacy";
        public string AuditEntityId => Id.ToString();

    }
}
