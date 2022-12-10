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
        var query = $"SELECT * FROM [Users] WHERE id = {id}";
        return DbInterface?.Select(query)?[0];
    }

    // TODO: Refactor all weird thing functions
    public bool VerifyUser(string email, string password)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface?.GetConnection();

        command.CommandText = "SELECT * FROM [Users] WHERE email=@email";
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
    
    public int WeirdThingAgainId(string email)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface?.GetConnection();

        command.CommandText = "SELECT id FROM [Users] WHERE email=@email";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        return (int)command.ExecuteScalar();
    }

    public bool WeirdThingAdmin(string email)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface.GetConnection();

        command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND admin = 1";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        return command.ExecuteScalar() != null;
    }

    public bool WeirdThingTeacher(string email)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface.GetConnection();

        command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND teacher = 1";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        return command.ExecuteScalar() != null;
    }
}