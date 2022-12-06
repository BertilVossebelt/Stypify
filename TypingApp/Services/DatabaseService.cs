using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

    public SqlDataReader ExecuteRaw(string query)
    {
        var command = new SqlCommand(query, _connection);
        return command.ExecuteReader();
    }

    public List<Dictionary<string, object>> Select(string query)
    {
        var command = new SqlCommand(query, _connection);
        var result = command.ExecuteReader();
        var list = new List<Dictionary<string, object>>();
        
        while (result.Read())
        {
            var dict = new Dictionary<string, object>();
            for (var i = 0; i < result.FieldCount; i++)
            {
                dict.Add(result.GetName(i), result[i]);
            }
            
            list.Add(dict);
        }

        result.Close();
        return list;
    }
    
    public SqlDataReader ExecuteSqlStatement(string sqlQuery)
    {
        var command = new SqlCommand(sqlQuery, _connection);
        var reader = command.ExecuteReader();
        command.StatementCompleted += SqlCommand_StatementCompleted;

        return reader;
    }

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