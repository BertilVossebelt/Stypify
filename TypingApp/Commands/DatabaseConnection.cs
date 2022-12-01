using System;
using System.Data.SqlClient;
using Renci.SshNet;
using System.Data;

namespace TypingApp.Commands
{
    public class DatabaseConnection
    {
        private readonly SqlConnectionStringBuilder _builder = new();
        private readonly SqlConnection? _connection;

        public DatabaseConnection()
        {
            var cSsh = new SshClient("145.44.233.157", "student", "UB22TypApp");
            cSsh.Connect();

            var forwardedPortLocal = new ForwardedPortLocal("127.0.0.1", 1433, "127.0.0.1", 1433);
            cSsh.AddForwardedPort(forwardedPortLocal);
            forwardedPortLocal.Start();

            try
            {
                _builder.DataSource = "127.0.0.1"; // localhost
                _builder.UserID = "SA";
                _builder.Password = "<MSSQL22TypApp>";
                _builder.InitialCatalog = "typapp";

                _connection = new SqlConnection(_builder.ConnectionString);
                _connection.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public SqlDataReader ExecuteSqlStatement(string sqlQuery)
        {
            var command = new SqlCommand(sqlQuery, _connection);
            var reader = command.ExecuteReader();
            command.StatementCompleted += SqlCommand_StatementCompleted;

            return reader;
        }
        
        // TODO: "ExecuteSqlStatement>2<" not a great name.
        public void ExecuteSqlStatement2(string sqlQuery)
        {
            var command = new SqlCommand(sqlQuery, _connection);
            command.StatementCompleted += SqlCommand_StatementCompleted;
            int result = command.ExecuteNonQuery();

            // Check Error
            if (result < 0) Console.WriteLine("Error inserting data into Database!");
            else Console.WriteLine("gelukt " + result);
        }

        private static void SqlCommand_StatementCompleted(object sender, StatementCompletedEventArgs e)
        {
            Console.WriteLine($"Records changed:{e.RecordCount}");
        }

        public SqlConnection? GetConnection()
        {
            return _connection;
        }
    }
}