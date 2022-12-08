using System;
using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public class GroupProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [Groups] WHERE id = {id}";
        return DbInterface?.Select(query)?[0];
    }

    public Dictionary<string, object>? GetByCode(string groupCode)
    {
        Console.WriteLine(groupCode);
        var query = $"SELECT * FROM [Groups] WHERE code = '{groupCode}';";
        return DbInterface?.Select(query)?[0];
    }

    public List<Dictionary<string, object>>? GetStudents(int groupId)
    {
        var query = $"SELECT * FROM [Users] JOIN Group_Student ON Users.id = Group_Student.student_id WHERE Group_Student.group_id = {groupId}";
        return DbInterface?.Select(query);
    }
    
    public List<Dictionary<string, object>>? GetStudent(int groupId, int studentId)
    {
        var query = $"SELECT id FROM Group_Student WHERE group_id='{groupId}' AND student_id = {studentId}";
        return DbInterface?.Select(query);
    }

    public Dictionary<string, object>? Create(int teacherId, string groupName, string groupCode)
    {
        var query = $"INSERT INTO [Groups] (teacher_id,name,code) VALUES ({teacherId}, '{groupName}', '{groupCode}')";
        return DbInterface?.Insert(query);
    }

    public Dictionary<string, object>? LinkStudent(int groupId, int studentId)
    {
        var query = $"INSERT INTO Group_Student (group_id,student_id) VALUES ({groupId} , {studentId})";
        return DbInterface?.Select(query)?[0];
    }
}