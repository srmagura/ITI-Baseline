using ITI.DDD.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestApp.DataContext.DataModel
{
    public class DbLtcPharmacy : DbEntity
    {
        [MaxLength(64)]
        public string? Name { get; set; }
    }
}
