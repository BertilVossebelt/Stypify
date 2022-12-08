using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public class ExerciseProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var query = $"SELECT * FROM [Exercise] WHERE id = {id}";
        return DbInterface?.Select(query)?[0];
    }
    
    public  Dictionary<string, object>? Create(int teacherId, string exerciseName, string exerciseText)
    {
        var query = $"INSERT INTO [Exercise] (teacher_id, name, text) VALUES ({teacherId}, '{exerciseName}', '{exerciseText}')";
        return DbInterface?.Insert(query);
    }
    
    
}