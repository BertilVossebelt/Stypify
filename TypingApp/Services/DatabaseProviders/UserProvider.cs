using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TypingApp.Models;

namespace TypingApp.Services.DatabaseProviders;

public class UserProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [User] WHERE id = {id}";
        return DbInterface?.Select(query)?[0];
    }

    // Check if user credentials are correct.
    public bool VerifyUser(string email, string password)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface?.GetConnection();
        command.CommandText = "SELECT hashedpassword, salt FROM [User] WHERE email=@email";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;

        using var reader = command.ExecuteReader();
        if (reader == null) return false;
        while (reader.Read())
        {
            var hashedPassword = (byte[])reader["hashedpassword"];
            var salt = (byte[])reader["salt"];
            var hash = new PasswordHash(hashedPassword);
            return hash.Verify(password, salt);
        }
        return false;
    }
    
    // Get id of the user by email.
    public int GetUserId(string email)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface?.GetConnection();

        command.CommandText = "SELECT id FROM [User] WHERE email=@email";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        return (int)command.ExecuteScalar();
    }
}