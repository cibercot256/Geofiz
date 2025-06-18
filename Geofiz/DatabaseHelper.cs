using System.Data;
using Microsoft.Data.SqlClient;

namespace GeofizApp
{
    public static class DatabaseHelper
    {
        private static readonly string connStr = "Data Source=DESKTOP-4D4RQJO;Initial Catalog=Uhalov;Integrated Security=True;TrustServerCertificate=True;";


        public static DataTable ExecuteQuery(string query)
        {
            using SqlConnection conn = new(connStr);
            using SqlCommand cmd = new(query, conn);
            using SqlDataAdapter da = new(cmd);
            DataTable dt = new();
            da.Fill(dt);
            return dt;
        }

        public static int ExecuteNonQuery(string query)
        {
            using SqlConnection conn = new(connStr);
            using SqlCommand cmd = new(query, conn);
            conn.Open();
            return cmd.ExecuteNonQuery();
        }
    }
}