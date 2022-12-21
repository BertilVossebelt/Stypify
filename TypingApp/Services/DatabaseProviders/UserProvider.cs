using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using TypingApp.Models;

namespace TypingApp.Services.DatabaseProviders;

public class UserProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [User] WHERE id = {id}";
        return DbInterface?.Select(query)?[0];
    }

    // Check of de verificatie van de user correct is.
    public bool VerifyUser(string email, string password)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface?.GetConnection();

        command.CommandText = "SELECT hashedpassword, salt FROM [User] WHERE email=@email";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        
        using (SqlDataReader reader = command.ExecuteReader())
        {
            if (reader != null)
            {
                while (reader.Read())
                {
                    byte[] hashedpassword = (byte[])reader["hashedpassword"];
                    byte[] salt = (byte[])reader["salt"];
                    PasswordHash hash = new PasswordHash(hashedpassword);
                    return (hash.Verify(password, salt));
                }
            }
            return false;
        }
    }
    
    // Check welk userID bij deze email hoort.
    public int GetUserId(string email)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface?.GetConnection();

        command.CommandText = "SELECT id FROM [User] WHERE email=@email";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        return (int)command.ExecuteScalar();
    }
}