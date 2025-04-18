using System;
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

        public DataTable GetDeviceTypes()
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from DeviceTypes", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }

            return table;
        }

        public DataTable GetDevicePassports()
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from DevicePassport", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
            }

            return table;
        }

        public DataTable GetAgreements()
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from RentalAgreements", connection);
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

        public bool AddAgreement(string number, DateTime agreementDate, DateTime startDate, DateTime endDate, int clientId, int deviceId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(
                    "insert into RentalAgreements(AgreementNumber, AgreementDate, RentStartDate, RentEndDate, ClientId, DevicePassportId) " +
                    "values (@Number, @AgreementDate, @StartDate, @EndDate, @ClientId, @DeviceId)", connection);

                command.Parameters.AddWithValue("@Number", number);
                command.Parameters.AddWithValue("@AgreementDate", agreementDate);
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);
                command.Parameters.AddWithValue("@ClientId", clientId);
                command.Parameters.AddWithValue("@DeviceId", deviceId);

                connection.Open();
                int result = command.ExecuteNonQuery();
                return result > 0;

            }
        }
    }
}
