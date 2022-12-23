using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public class LessonProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT * FROM Lessons WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader)?[0];
    }
    
    public  List<Dictionary<string, object>?>? GetExercises(int lessonId)
    {
        var cmd = GetSqlCommand();
        cmd.CommandText = "SELECT e.id, e.name, e.text FROM [Exercise] e JOIN [Lesson_Exercise] le ON e.id = le.exercise_id WHERE le.lesson_id = @lessonId";
        cmd.Parameters.AddWithValue("@lessonId", lessonId);
        var reader = cmd.ExecuteReader();
        
        return ConvertToList(reader);
    }
}