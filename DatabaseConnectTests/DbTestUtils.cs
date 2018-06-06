using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    static class DbTestUtils
    {
        internal static void ClearTables(params string[] tableNames)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionStringsProvider.GetTest()))
            {
                conn.Open();

                foreach (string tableName in tableNames)
                {
                    SqlCommand cmd = new SqlCommand(GetTableClearCommandString(tableName), conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string GetTableClearCommandString(string tableName)
        {
            return string.Format("DELETE FROM [{0}];", tableName);
        }
    }
}
