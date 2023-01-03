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
        cmd.CommandText = "SELECT * FROM [User] WHERE id = @id AND admin = 1";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader, "AdminProvider.GetById")?[0];
    }
    
    // Haalt alle teachers op voor in het admindashboard.
    public List<Dictionary<string, object>?>? GetTeachers()
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE teacher = 1";
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader);
    }
    
    // Verwijdert een teacher.
    public Dictionary<string, object> RemoveTeacher(string email)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "DELETE FROM [User] WHERE email = @email";
        cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader)?[0];
    }
}