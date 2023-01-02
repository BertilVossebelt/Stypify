using System.Collections.Generic;
using System.Data;
using System.Security;

namespace TypingApp.Services.DatabaseProviders;

public class AdminProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE id = @id AND admin = 1";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader, "AdminProvider.GetById")?[0];
    }

    // TODO: This should e be removed, the create teacher method in the teacher provider should be used instead.
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