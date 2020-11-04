using ITI.Baseline.ValueObjects;
using ITI.DDD.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TestApp.Domain.ValueObjects;

namespace TestApp.DataContext.DataModel
{
    public class DbCustomer : DbEntity
    {
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string? NotInEntity { get; set; }

        public SimplePersonName? SimplePersonName { get; set; }
        public PhoneNumber? PhoneNumber { get; set; }
        public SimpleAddress? SimpleAddress { get; set; }

       // public List<DbBar> Bars { get; set; }

        //public string SomeInts { get; set; }

        [Column(TypeName = "Money")]
        public decimal SomeMoney { get; set; }

        //public string SomeGuids { get; set; }

        public long SomeNumber { get; set; }
    }
}
