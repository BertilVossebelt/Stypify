using System.Collections.Generic;

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
    
    public Dictionary<string, object>? GetByEmail(string email)
    {
        var query = $"SELECT * FROM [Groups] WHERE {email}";
        return DbInterface?.Select(query)?[0];
    }

}