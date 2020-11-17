using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.DataContext.DataModel
{
    public class DbCustomerUser : DbUser
    {
        public Guid CustomerId { get; set; }
    }
}
