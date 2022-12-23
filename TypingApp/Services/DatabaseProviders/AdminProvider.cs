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
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM Admin WHERE id = @id AND admin = 1";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader)?[0];
    }

    // TODO: Refactor to make heavier use of DbInterface. Function is currently too complicated.
    public Dictionary<string, object>? RegisterTeacher(string email, SecureString password, string? preposition, string firstName, string lastName)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [User] (email, password, first_name, preposition, last_name, teacher, admin)" +
                          "VALUES (@email, @password, @first_name, @preposition, @last_name, 1, 0)";
        cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
        cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
        cmd.Parameters.Add("@first_name", SqlDbType.VarChar).Value = firstName;
        cmd.Parameters.Add("@preposition", SqlDbType.VarChar).Value = preposition;
        cmd.Parameters.Add("@last_name", SqlDbType.VarChar).Value = lastName;
        var id = (int)cmd.ExecuteScalar();
        
        return GetById(id);
    }
}