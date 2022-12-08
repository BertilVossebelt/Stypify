using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TypingApp.Services.DatabaseProviders;

public class TeacherProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [Users] WHERE id = {id} AND teacher = true";
        return DbInterface?.Select(query)?[0];
    }

    public IEnumerable<Dictionary<string, object>>? GetGroups(int teacherId)
    {
        var query = $"SELECT * FROM [Groups] WHERE teacher_id = {teacherId}";
        return DbInterface?.Select(query);
    }
    
    // TODO: Refactor
    public Dictionary<string, object>? GetByEmail(string email)
    {
        var query = $"SELECT * FROM [Groups] WHERE email=@email";
        return DbInterface?.Select(query)[0];
    }

}