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

    public List<Dictionary<string, object>>? Select(string query)
    {
        var command = new SqlCommand(query, _connection);
        var reader = command.ExecuteReader();

        if (!reader.HasRows)
        {
            reader.Close();
            return null;
        }

        var list = new List<Dictionary<string, object>>();
        while (reader.Read())
        {
            var dict = new Dictionary<string, object>();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                dict.Add(reader.GetName(i), reader[i]);
            }

            list.Add(dict);
        }
        
        reader.Close();
        return list;
    }

    public Dictionary<string, object>? Insert(string query)
    {
        query += "; SELECT SCOPE_IDENTITY()";
        var command = new SqlCommand(query, _connection);
        var id = command.ExecuteScalar();
        var dict = new Dictionary<string, object>();
        
        // Get table from query
        var pFrom  = query.IndexOf("INSERT INTO [", StringComparison.OrdinalIgnoreCase) + "INSERT INTO [".Length;
        var pTo  = query.LastIndexOf("] (", StringComparison.OrdinalIgnoreCase);
        var table = query.Substring(pFrom, pTo - pFrom);

        // Get inserted record
        query = $"SELECT * FROM [{table}] WHERE id = '{id}'"; 
        command = new SqlCommand(query, _connection);
        var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                dict.Add(reader.GetName(i), reader[i]);
            }
        }
    
        reader.Close();
        return dict;
    }
    
    public SqlConnection? GetConnection()
    {
        return _connection;
    }
}