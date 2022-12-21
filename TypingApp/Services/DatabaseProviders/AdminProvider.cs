using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security;

namespace TypingApp.Services.DatabaseProviders;

public class AdminProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [Users] WHERE id = {id} AND admin = true";
        return DbInterface?.Select(query)?[0];
    }

    // TODO: Refactor to make heavier use of DbInterface. Function is currently too complicated.
    public int RegisterTeacher(string email, SecureString password, string? preposition, string firstName, string lastName)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface?.GetConnection();

        command.CommandText =
            "INSERT INTO [Users] (teacher, student, email, password, first_name, preposition, last_name, admin)" +
            "VALUES (@teacher, @student, @email, @password, @first_name, @preposition, @last_name, @admin)";

        command.Parameters.Add("@teacher", SqlDbType.TinyInt).Value = 1;
        command.Parameters.Add("@student", SqlDbType.TinyInt).Value = 0;
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;

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