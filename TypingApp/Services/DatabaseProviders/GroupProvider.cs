using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls.Primitives;
using TypingApp.Models;

namespace TypingApp.Services.DatabaseProviders;

public class GroupProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Group] WHERE id = @id";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "GroupProvider.GetById")?[0];
    }

    public Dictionary<string, object>? GetByCode(string groupCode)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Group] WHERE code = @code";
        cmd.Parameters.AddWithValue("@code", groupCode);     
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "GroupProvider.GetByCode")?[0];
    }

    public List<Dictionary<string, object>>? GetStudentLessons(int groupId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT l.id, l.name, l.teacher_id, l.image, ul.completed FROM [Lesson] l JOIN [Group_Lesson] gl ON l.id = gl.lesson_id JOIN [User_Lesson] ul ON l.id = ul.lesson_id WHERE gl.group_id = @groupId";
        cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "GroupProvider.GetLessons");
    }

    public List<Dictionary<string, object>>? GetLessons(int groupId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT l.id, l.name, l.teacher_id, l.image FROM [Lesson] l JOIN [Group_Lesson] gl ON l.id = gl.lesson_id WHERE gl.group_id = @groupId";
        cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader, "GroupProvider.GetLessons");
    }

    public List<Dictionary<string, object>>? GetUncompletedLessons(int groupId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT l.id, l.name, l.teacher_id, l.image, ul.completed FROM [Lesson] l JOIN [Group_Lesson] gl ON l.id = gl.lesson_id JOIN [User_Lesson] ul ON l.id = ul.lesson_id WHERE gl.group_id = @groupId AND ul.completed = @bool";
        cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
        cmd.Parameters.Add("@bool", SqlDbType.TinyInt).Value = 0;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader, "GroupProvider.GetUncompletedLessons");
    }
    public List<Dictionary<string, object>>? GetStudents(int groupId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT u.id, u.teacher, u.email, u.first_name, u.preposition, u.last_name, u.admin, s.completed_exercises " +
                          "FROM [User] u JOIN Group_Student gs ON u.id = gs.student_id " +
                          "LEFT JOIN [Student] s ON gs.student_id = s.student_id" +
                          " WHERE gs.group_id = @groupId";
        cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "GroupProvider.GetStudents");
    }
    
    public List<Dictionary<string, object>>? GetStudentById(int groupId, int studentId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Group_Student] WHERE group_id = @groupId AND student_id = @studentId";
        cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
        cmd.Parameters.Add("@studentId", SqlDbType.Int).Value = studentId;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader,  "GroupProvider.GetStudentById");
    }

    public Dictionary<string, object>? Create(int teacherId, string groupName, string groupCode)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [Group] (teacher_id, name, code) VALUES (@teacherId, @groupName, @groupCode); SELECT SCOPE_IDENTITY()";
        cmd.Parameters.Add("@teacherId", SqlDbType.Int).Value = teacherId;
        cmd.Parameters.Add("@groupName", SqlDbType.NVarChar).Value = groupName;
        cmd.Parameters.Add("@groupCode", SqlDbType.NVarChar).Value = groupCode;
        var id = (decimal)cmd.ExecuteScalar();
        
        return GetById((int)id);
    }


    public Dictionary<string, object>? UpdateGroupCode(int groupId, string groupCode)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "UPDATE [Group] SET code = @groupCode WHERE id = @groupId";
        cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
        cmd.Parameters.Add("@groupCode", SqlDbType.NVarChar).Value = groupCode;
        cmd.ExecuteNonQuery();

        return GetById(groupId);
    }
}