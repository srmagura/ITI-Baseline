using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTests.Harness
{
    public static class IntegrationTestData
    {
        public static void ResetDb(string connectionString)
        {
            DeleteFromTablesUtil.DeleteFromTables(connectionString);
        }
    }
}
