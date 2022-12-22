using System;
using System.Collections.Generic;
using System.Data;
using System.Security;
using System.Text;

namespace TypingApp.Services.DatabaseProviders;

public class TeacherProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [User] WHERE id = {id} AND teacher = 1";
        return DbInterface?.Select(query)?[0];
    }

    public IEnumerable<Dictionary<string, object>>? GetGroups(int teacherId)
    {
        var query = $"SELECT * FROM [Group] WHERE teacher_id = {teacherId}";
        return DbInterface?.Select(query);
    }
    
    public List<Dictionary<string, object>>? GetLessons(int teacherId)
    {
        var query = $"SELECT * FROM Lessons WHERE teacherId = '{teacherId}'";
        return DbInterface?.Select(query);
    }

    public Dictionary<string, object>? GetByEmail(string email)
    {
        var query = $"SELECT * FROM [Group] WHERE {email}";
        return DbInterface?.Select(query)?[0];
    }

    public Dictionary<string, object>? Create(string email, byte[] password, byte[] salt, string firstName, string? preposition, string lastName)
    {
        var query = $"INSERT INTO [User] (teacher, email, password, salt, first_name, preposition, last_name, admin) VALUES (1, '{email}', CONVERT(VARBINARY, '{password}'), CONVERT(VARBINARY, '{salt}'), '{firstName}', '{preposition}', '{lastName}', 0)";
        
        return DbInterface?.SafeInsert(query);
    }
}