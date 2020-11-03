using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.DataContext
{
    public class ConnectionStrings
    {
        public string DefaultDataContext { get; set; } = "Server=localhost;Database=ITIBaseline_e2e_test;Trusted_Connection=True;Connection Timeout=180;MultipleActiveResultSets=True;";
    }
}
