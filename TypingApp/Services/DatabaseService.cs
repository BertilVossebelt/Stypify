using System;
using System.Data.SqlClient;
using Renci.SshNet;

namespace TypingApp.Services;

public class DatabaseService
{
    private readonly SqlConnectionStringBuilder _builder = new();
    private readonly SqlConnection? _connection;
    private readonly SshClient? _sshClient;

    /*
     * This service handles the connection to the database.
     */
    public DatabaseService()
    {
        // Create a new SSH client.
        _sshClient = new SshClient("145.44.233.157", "student", "UB22TypApp");
        _sshClient.Connect();

        // Create a local port forward to the database.
        var forwardedPortLocal = new ForwardedPortLocal("127.0.0.1", 1433, "127.0.0.1", 1433);
        _sshClient.AddForwardedPort(forwardedPortLocal);
        forwardedPortLocal.Start();

        try
        {
            // Connection details.
            _builder.DataSource = "127.0.0.1";
            _builder.UserID = "SA";
            _builder.Password = "<MSSQL22TypApp>";
            _builder.InitialCatalog = "typapp";
            _builder.TransactionBinding = "Explicit Unbind";

            // Create a new connection.
            _connection = new SqlConnection(_builder.ConnectionString);
            _connection.Open();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    
    public void CloseConnection()
    {
        _sshClient?.Disconnect();
        _connection?.Dispose();
    }
    
    public SqlConnection? GetConnection()
    {
        return _connection;
    }
}