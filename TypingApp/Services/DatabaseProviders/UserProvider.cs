using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TypingApp.Models;

namespace TypingApp.Services.DatabaseProviders;

public class UserProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE id = @id";
        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = id;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader)?[0];
    }

    public Dictionary<string, object>? GetByCredentials(string email)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE email = @email";
        cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar)).Value = email;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader)?[0];
    }
}