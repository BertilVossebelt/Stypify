using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public class LessonProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        const string query = "SELECT * FROM Lessons WHERE Id = id";
        return DbInterface?.Select(query)?[0];
    }
    
    public  List<Dictionary<string, object>>? GetExercises(int lessonId)
    {
        var query = $"SELECT e.id, e.name, e.text FROM [Exercise] e JOIN [Lesson_Exercise] le ON e.id = le.exercise_id WHERE le.lesson_id = '{lessonId}'";
        return DbInterface?.Select(query);
    }
}