using System.Collections.Generic;
using System.Data;

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
    
    // Removes a teacher.
    public Dictionary<string, object>? Delete(string email)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "DELETE FROM [User] WHERE email = @email";
        cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader, "AdminProvider.RemoveTeacher")?[0];
    }
}