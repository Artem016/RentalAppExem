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

        public void AddClint(string lastName, string firstName, string middleName, string passportSeries, string passportNumber, string address)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "insert into Clients (LastName, FirstName, MiddleName, PassportSeries, PassportNumber, Address) " +
                    "values (@LastName, @FirstName, @MiddleName, @PassportSeries, @PassportNumber, @Address)", connection);

                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@MiddleName", middleName);
                command.Parameters.AddWithValue("@PassportSeries", passportSeries);
                command.Parameters.AddWithValue("@PassportNumber", passportNumber);
                command.Parameters.AddWithValue("@Address", address);

                command.ExecuteNonQuery();

            }
        }
    }
}
