using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TypingApp.Models;

namespace TypingApp.Services.DatabaseProviders;

public class ExerciseProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Exercise] WHERE id = @id";
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader)?[0];
    }

    public Dictionary<string, object>? Create(int teacherId, string exerciseName, string exerciseText)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "INSERT INTO [Exercise] (teacher_id, name, text) VALUES (@teacherId, @exerciseName, @exerciseText); SELECT SCOPE_IDENTITY()";
        cmd.Parameters.Add("@teacherId", SqlDbType.Int).Value = teacherId; 
        cmd.Parameters.Add("@exerciseName", SqlDbType.NVarChar).Value = exerciseName;
        cmd.Parameters.Add("@exerciseText", SqlDbType.NVarChar).Value = exerciseText;
        var id = (decimal)cmd.ExecuteScalar();
        
        return GetById((int)id);
    }

    public List<Dictionary<string, object>?>? GetAll(int teacherId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM [Exercise] WHERE teacher_id = @teacherId";
        cmd.Parameters.Add("@teacherId", SqlDbType.Int).Value = teacherId;
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader);
    }
}