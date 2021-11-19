using ITI.Baseline.Audit;
using ITI.DDD.Infrastructure.DataContext;
using System.ComponentModel.DataAnnotations;
using TestApp.Domain.ValueObjects;

namespace TestApp.DataContext.DataModel
{
    public class DbFacility : DbEntity, IDbAudited
    {
        public DbFacility(string? name, FacilityContact contact)
        {
            Name = name;
            Contact = contact;
        }

        [MaxLength(64)]
        public string? Name { get; set; }

        public FacilityContact Contact { get; set; }

        public string AuditEntityName => "Facility";
        public string AuditEntityId => Id.ToString();
    }
}
