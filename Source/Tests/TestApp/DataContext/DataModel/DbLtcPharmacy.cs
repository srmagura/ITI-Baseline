using ITI.Baseline.Audit;
using ITI.DDD.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

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

        public static void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<DbLtcPharmacy>()
                .HasIndex(p => p.Name)
                .IsUnique();
        }
    }
}
