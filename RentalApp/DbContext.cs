using System.Data;
using System.Data.SqlClient;

namespace RentalApp
{
    internal class DbContext
    {
        private readonly string _connectionString = "Server=HOME-PC\\SQLEXPRESS;Database=RentalDB;TrustServerCertificate=True;Trusted_Connection=True;";

        public DataTable GetClints()
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from Clients", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }

            return table;
        }
    }
}
