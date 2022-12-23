using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TypingApp.Services.DatabaseProviders;

public class StudentProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int studentId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE id = @studentId AND teacher = 0 AND admin = 0";
        cmd.Parameters.Add("@studentId", SqlDbType.Int).Value = studentId;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader)?[0];
    }
    
    public Dictionary<string, object>? GetByEmail(string email)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE email = @email AND teacher = 0 AND admin = 0";
        cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader)?[0];
    }
    
    public Dictionary<string, object>? GetLessonById(int lessonId, int studentId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User_Lesson] WHERE lesson_id = @lessonId AND student_id = @studentId";
        cmd.Parameters.Add("@lessonId", SqlDbType.Int).Value = lessonId;
        cmd.Parameters.Add("@studentId", SqlDbType.Int).Value = studentId;
        var reader = cmd.ExecuteReader();

        return ConvertToList(reader)?[0];
    }
    
    public Dictionary<string, object>? UpdateLesson(int lessonId, int studentId, int placeNumber)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "UPDATE [User_Lesson] SET place_number = @placeNumber WHERE lesson_id = @lessonId AND student_id = @studentId";
        cmd.Parameters.Add("@lessonId", SqlDbType.Int).Value = lessonId;
        cmd.Parameters.Add("@studentId", SqlDbType.Int).Value = studentId;
        cmd.Parameters.Add("@placeNumber", SqlDbType.Int).Value = placeNumber;
        cmd.ExecuteNonQuery();
        
        return GetLessonById(lessonId, studentId);
    }
    
    public Dictionary<string, object>? CreateLesson(int lessonId, int studentId, int placeNumber)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [User_Lesson] (lesson_id, student_id, place_number) VALUES (@lessonId, @studentId, @placeNumber); SELECT SCOPE_IDENTITY()";
        cmd.Parameters.Add("@lessonId", SqlDbType.Int).Value = lessonId;
        cmd.Parameters.Add("@studentId", SqlDbType.Int).Value = studentId;
        cmd.Parameters.Add("@placeNumber", SqlDbType.Int).Value = placeNumber;
        cmd.ExecuteScalar();
        
        return GetLessonById(lessonId, studentId);
    }
    
    public List<Dictionary<string, object>>? GetGroups(int studentId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT g.id, g.name, g.teacher_id, g.image FROM [Group] g JOIN [Group_Student] gl ON g.id = gl.group_id WHERE student_id = @studentId";
        cmd.Parameters.Add("@studentId", SqlDbType.Int).Value = studentId;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader);
    }
    
    public Dictionary<string, object>? Create(string email, byte[] password, byte[] salt, string firstName, string? preposition, string lastName)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [User] (email, password, salt, first_name, preposition, last_name, teacher, admin) " +
                          "VALUES (@email, @password, @salt, @firstName, @preposition, @lastName, 0, 0); SELECT SCOPE_IDENTITY()";
        cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
        cmd.Parameters.Add("@password", SqlDbType.VarBinary).Value = password;
        cmd.Parameters.Add("@salt", SqlDbType.VarBinary).Value = salt;
        cmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = firstName;
        cmd.Parameters.Add("@preposition", SqlDbType.VarChar).Value = string.IsNullOrEmpty(preposition) ? DBNull.Value : preposition;
        cmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = lastName;
        var id = (decimal)cmd.ExecuteScalar();
        
        return GetById((int)id);
    }
    
    public Dictionary<string, object>? LinkToGroup(int groupId, int studentId)
    {
        // Create link between student and group.
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [Group_Student] (group_id, student_id) VALUES (@groupId, @studentId); SELECT SCOPE_IDENTITY()";
        cmd.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;
        cmd.Parameters.Add("@studentId", SqlDbType.Int).Value = studentId;
        var id = (decimal)cmd.ExecuteScalar();
        
        // Retrieve the newly created link.
        cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Group_Student] WHERE id = @id";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader)?[0];
    }
}