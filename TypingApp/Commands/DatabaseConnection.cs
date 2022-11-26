using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Renci.SshNet;

namespace TypingApp.Commands
{
    public class DatabaseConnection
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        SqlConnection connection;
        public DatabaseConnection()
        {
            SshClient cSSH = new SshClient("145.44.233.157", "student", "UB22TypApp");

            cSSH.Connect();
            ForwardedPortLocal forwardedPortLocal = new ForwardedPortLocal("127.0.0.1", 1433, "127.0.0.1", 1433);
            cSSH.AddForwardedPort(forwardedPortLocal);
            forwardedPortLocal.Start();
            try
            {
                builder.DataSource = "127.0.0.1"; //localhost
                builder.UserID = "SA";
                builder.Password = "<MSSQL22TypApp>";
                builder.InitialCatalog = "typapp";
                connection = new SqlConnection(builder.ConnectionString);
                connection.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public SqlDataReader ExecuteSqlStatement(String sqlQuery)
        {
            SqlCommand command = new SqlCommand(sqlQuery, connection);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }
    }
}
