using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.DataContext.DataModel
{
    public class DbOnCallUser : DbUser
    {
        public Guid OnCallProviderId { get; set; }
    }
}
