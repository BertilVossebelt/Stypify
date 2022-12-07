using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TypingApp.Services.DatabaseProviders;

public class UserProvider : BaseProvider
{
    public override List<Dictionary<string, object>>? GetById(int id)
    {
        var query = $"SELECT * FROM users WHERE id = {id}";
        return DbConnection?.Select(query);
    }

    public bool WeirdThing(string username, string password)
    {
        var command = new SqlCommand();
        command.Connection = DbConnection.GetConnection();

        command.CommandText = "SELECT * FROM [Users] WHERE email=@email and [password]=@password";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = username;
        command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
        return command.ExecuteScalar() != null;
    }
    public int WeirdThingAgainId(string username, string password)
    {
        var command = new SqlCommand();
        command.Connection = DbConnection.GetConnection();

        command.CommandText = "SELECT * FROM [Users] WHERE email=@email and [password]=@password";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = username;
        command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
        return (int)command.ExecuteScalar();
    }

    public bool WeirdThingAdmin(string email)
    {
        var command = new SqlCommand();
        command.Connection = DbConnection.GetConnection();

        command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND admin = 1";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        return command.ExecuteScalar() != null;
    }

    public bool WeirdThingTeacher(string email)
    {
        var command = new SqlCommand();
        command.Connection = DbConnection.GetConnection();

        command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND teacher = 1";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        return command.ExecuteScalar() != null;
    }
}