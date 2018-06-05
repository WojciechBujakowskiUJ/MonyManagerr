using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConnect
{
    public class SqlService
    {

        public static string ConnectionString { get; set; }

        public static DataTable GetDataTable(SqlQueryBuilder query)
        {
            return GetDataTable(query.GetSql, query.GetParams);
        }

        public static DataTable GetDataTable(string query, SqlParameter[] parametrs)
        {
            DataTable t1 = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(parametrs);
                using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                {
                    a.Fill(t1);
                }
            }
            return t1;
        }

        public static int ExecuteScalar(string query, SqlParameter[] parametrs)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(parametrs);
                return (int)cmd.ExecuteScalar();
            }
        }

        public static void ExecuteNonQuery(string query, SqlParameter[] parametrs)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(parametrs);
                cmd.ExecuteNonQuery();
            }
        }

    }

}