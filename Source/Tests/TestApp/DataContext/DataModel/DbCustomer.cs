using ITI.Baseline.Audit;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Infrastructure.DataContext;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestApp.Domain.ValueObjects;

namespace TestApp.DataContext.DataModel
{
    public class DbCustomer : DbEntity, IDbAudited
    {
        public DbCustomer(string name, decimal someMoney, long someNumber)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SomeMoney = someMoney;
            SomeNumber = someNumber;
        }

        [MaxLength(64)]
        public string Name { get; set; }

        public SimpleAddress? Address { get; set; }
        public SimplePersonName? ContactName { get; set; }
        public PhoneNumber? ContactPhone { get; set; }

        public List<DbLtcPharmacy> LtcPharmacies { get; set; } = new List<DbLtcPharmacy>();
        public string SomeInts { get; set; } = "[]";

        [Column(TypeName = "Money")]
        public decimal SomeMoney { get; set; }

        public long SomeNumber { get; set; }

        public string AuditEntityName => "Customer";
        public string AuditEntityId => Id.ToString();
    }
}
