using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IntegrationTests.Harness
{
    public static class DeleteFromTablesUtil
    {
        public static void DeleteFromTables(string connectionString)
        {
            DeleteFromTables(connectionString, new List<string>() { "QuickCommentNodes" });
        }

        private static void DeleteFromTables(string connectionString, List<string> except)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var tableNames = GetTableNames(conn, except);

                try
                {
                    {
                        // disable constraints
                        var cmdText = "EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? NOCHECK CONSTRAINT all'";
                        using (var cmd = new SqlCommand(cmdText, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }

                    foreach (var tableName in tableNames)
                    {
                        var cmdText = $"DELETE FROM {tableName}";
                        using (var cmd = new SqlCommand(cmdText, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                finally
                {
                    // enable constraints
                    var cmdText = "EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? CHECK CONSTRAINT all'";
                    using (var cmd = new SqlCommand(cmdText, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private static List<string> GetTableNames(SqlConnection conn, List<string> except)
        {
            var tableNames = new List<string>();

            var cmdText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES " +
                      "WHERE TABLE_TYPE = 'BASE TABLE'";

            using (var cmd = new SqlCommand(cmdText, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tableNames.Add(reader.GetString(0));
                    }
                }
            }

            except = except.Concat(new List<string> { "__EFMigrationsHistory" }).ToList();
            return tableNames.Except(except).ToList();
        }
    }
}
