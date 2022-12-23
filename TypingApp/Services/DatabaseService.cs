using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Renci.SshNet;

namespace TypingApp.Services;

public class DatabaseService
{
    private readonly SqlConnectionStringBuilder _builder = new();
    private readonly SqlConnection? _connection;

    public DatabaseService()
    {
        var cSsh = new SshClient("145.44.233.157", "student", "UB22TypApp");
        cSsh.Connect();

        var forwardedPortLocal = new ForwardedPortLocal("127.0.0.1", 1433, "127.0.0.1", 1433);
        cSsh.AddForwardedPort(forwardedPortLocal);
        forwardedPortLocal.Start();

        try
        {
            _builder.DataSource = "127.0.0.1";
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
    
    public void Insert()
    {
        // ; SELECT SCOPE_IDENTITY();
    }
    
    public SqlConnection? GetConnection()
    {
        return _connection;
    }
}