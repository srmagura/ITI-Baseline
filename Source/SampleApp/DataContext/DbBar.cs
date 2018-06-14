using System;
using System.ComponentModel.DataAnnotations;
using Iti.Core.DataContext;

namespace DataContext
{
    public class DbBar : DbEntity
    {
        public Guid FooId { get; set; }
        public DbFoo Foo { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string NotInEntity { get; set; }
    }
}