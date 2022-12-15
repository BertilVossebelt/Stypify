using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public class TeacherProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [User] WHERE id = {id} AND teacher = true";
        return DbInterface?.Select(query)?[0];
    }

    public IEnumerable<Dictionary<string, object>>? GetGroups(int teacherId)
    {
        var query = $"SELECT * FROM [Group] WHERE teacher_id = {teacherId}";
        return DbInterface?.Select(query);
    }
    
    public Dictionary<string, object>? GetByEmail(string email)
    {
        var query = $"SELECT * FROM [Group] WHERE {email}";
        return DbInterface?.Select(query)?[0];
    }

    public Dictionary<string, object>? Create(string email, string password, string firstName, string preposition, string lastName)
    {
        var query = $"INSERT INTO [User] (teacher, email, password, first_name, preposition, last_name, admin) VALUES (true, '{email}', '{password}', '{firstName}', '{preposition}', '{lastName}', false)";
        return DbInterface?.Insert(query);
    }
    
    public Dictionary<string, object>? Create(string email, string password, string firstName, string lastName)
    {
        var query = $"INSERT INTO [User] (teacher, email, password, first_name, last_name, admin) VALUES (1, '{email}', '{password}', '{firstName}', '{lastName}', 0)";
        return DbInterface?.Insert(query);
    }
    

}