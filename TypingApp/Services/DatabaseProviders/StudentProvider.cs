using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security;

namespace TypingApp.Services.DatabaseProviders;

public class StudentProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [Users] WHERE id = {id} AND teacher = false AND admin = false";
        return DbInterface?.Select(query)[0];
    }
    
    public Dictionary<string, object>? GetByEmail(string email)
    {
        var query = $"SELECT * FROM [Users] WHERE email= '{email}'";
        return DbInterface?.Select(query)?[0];
    }
    
    public int Create(string email, byte[] password, byte[] salt, string? preposition, string firstName, string lastName)
    {
        SqlCommand command = new SqlCommand();
        command.Connection = DbInterface.GetConnection();

        command.CommandText =
            "INSERT INTO [Users] (teacher, student, email, hashedpassword, salt, first_name, preposition, last_name, admin)" +
            "VALUES (@teacher, @student, @email, @hashedpassword, @salt, @first_name, @preposition, @last_name, @admin)";

        command.Parameters.Add("@teacher", SqlDbType.TinyInt).Value = 0;
        command.Parameters.Add("@student", SqlDbType.TinyInt).Value = 1;
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        command.Parameters.Add("@hashedpassword", SqlDbType.VarBinary).Value = password;
        command.Parameters.Add("@salt", SqlDbType.VarBinary).Value = salt;

        if (!string.IsNullOrEmpty(preposition))
        {
            command.Parameters.Add("@preposition", SqlDbType.NVarChar).Value = preposition;
        }
        else
        {
            command.Parameters.AddWithValue("@preposition", DBNull.Value);
        }

        command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = firstName;
        command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = lastName;
        command.Parameters.Add("@admin", SqlDbType.TinyInt).Value = 0;
        
        return command.ExecuteNonQuery();
    }
}