using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Domain.Identities;

namespace TestApp.DataContext.DataModel
{
    public class DbCustomerUser : DbUser
    {
        public Guid CustomerId { get; set; }
    }
}
