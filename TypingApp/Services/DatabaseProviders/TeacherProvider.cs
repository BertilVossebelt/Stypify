using System;
using System.Collections.Generic;
using System.Data;

namespace TypingApp.Services.DatabaseProviders;

public class TeacherProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE id = @id AND teacher = 1";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "TeacherProvider.GetById")?[0];
    }

    public List<Dictionary<string, object>>? GetAll()
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE teacher = 1";
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "TeacherProvider.GetAll");
    }

    public List<Dictionary<string, object>>? GetGroups(int teacherId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Group] WHERE teacher_id = @teacher_id";
        cmd.Parameters.Add("@teacher_id", SqlDbType.Int).Value = teacherId;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "TeacherProvider.GetGroups");
    }
    
    public List<Dictionary<string, object>>? GetLessons(int teacherId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Lesson] WHERE teacher_id = @teacher_id";
        cmd.Parameters.Add("@teacher_id", SqlDbType.Int).Value = teacherId;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "TeacherProvider.GetLessons");
    }

    public Dictionary<string, object>? GetByEmail(string email)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [User] WHERE email = @email AND teacher = 1";
        cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader, "TeacherProvider.GetByEmail")?[0];
    }

    public Dictionary<string, object>? Create(string email, byte[] password, byte[] salt, string firstName, string? preposition, string lastName)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [User] (email, password, salt, first_name, preposition, last_name, teacher, admin) " +
                          "VALUES (@email, @password, @salt, @first_name, @preposition, @last_name, 1, 0); SELECT SCOPE_IDENTITY()";
        cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
        cmd.Parameters.Add("@password", SqlDbType.VarBinary).Value = password;
        cmd.Parameters.Add("@salt", SqlDbType.VarBinary).Value = salt;
        cmd.Parameters.Add("@first_name", SqlDbType.VarChar).Value = firstName;
        cmd.Parameters.Add("@preposition", SqlDbType.VarChar).Value = string.IsNullOrEmpty(preposition) ? DBNull.Value : preposition;
        cmd.Parameters.Add("@last_name", SqlDbType.VarChar).Value = lastName;
        var id = (decimal)cmd.ExecuteScalar();
        
        return GetById((int)id);
    }
}