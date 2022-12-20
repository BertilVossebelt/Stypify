using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace TypingApp.Services.DatabaseProviders;

public class StudentProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int studentId)
    {
        var query = $"SELECT * FROM [User] WHERE id = {studentId} AND teacher = false AND admin = false";
        return DbInterface?.Select(query)?[0];
    }
    
    public Dictionary<string, object>? GetByEmail(string email)
    {
        var query = $"SELECT * FROM [User] WHERE email= '{email}'";
        return DbInterface?.Select(query)?[0];
    }
    
    public Dictionary<string, object>? GetLessonById(int lessonId, int studentId)
    {
        var query = $"SELECT * FROM [User_Lesson] WHERE lesson_id = {lessonId} AND student_id = {studentId}";
        return DbInterface?.Select(query)?[0];
    }
    
    public void UpdateLesson(int lessonId, int studentId, int placeNumber)
    {
        var query = $"UPDATE [User_Lesson] SET place_number = {placeNumber} WHERE lesson_id = {lessonId} AND student_id = {studentId}";
        var reader = DbInterface?.ExecuteRaw(query);
        reader?.Close();
    }
    
    public Dictionary<string, object>? CreateLesson(int lessonId, int studentId, int placeNumber)
    {
        var query = $"INSERT INTO [User_Lesson] (student_id, lesson_id, place_number) VAlUES ({studentId}, {lessonId}, {placeNumber})";
        return DbInterface?.Select(query)?[0];
    }
    
    public List<Dictionary<string, object>>? GetGroups(int studentId)
    {
        var query = $"SELECT g.id, g.name, g.teacher_id, g.image FROM [Group] g JOIN [Group_Student] gl ON g.id = gl.group_id WHERE gl.student_id = '{studentId}'";
        return DbInterface?.Select(query);
    }
    
    public int Create(string email, byte[] password, byte[] salt, string? preposition, string firstName, string lastName)
    {
        var command = new SqlCommand();
        command.Connection = DbInterface?.GetConnection();

        command.CommandText =
            "INSERT INTO [User] (teacher, email, hashedpassword, salt, first_name, preposition, last_name, admin)" +
            "VALUES (@teacher, @email, @hashedpassword, @salt, @first_name, @preposition, @last_name, @admin)";

        command.Parameters.Add("@teacher", SqlDbType.TinyInt).Value = 0;
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        command.Parameters.Add("@hashedpassword", SqlDbType.VarBinary).Value = password;
        command.Parameters.Add("@salt", SqlDbType.VarBinary).Value = salt;

        if (!string.IsNullOrEmpty(preposition))
        {
            command.Parameters.Add("@preposition", SqlDbType.NVarChar).Value = preposition;
        }
        else
        {
            command.Parameters.AddWithValue("@preposition", DBNull.Value);
        }

        command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = firstName;
        command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = lastName;
        command.Parameters.Add("@admin", SqlDbType.TinyInt).Value = 0;
        
        return command.ExecuteNonQuery();
    }
}