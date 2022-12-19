using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public class LessonProvider : BaseProvider
{
    public override Dictionary<string, object>? GetById(int id)
    {
        const string query = "SELECT * FROM Lessons WHERE Id = id";
        return DbInterface?.Select(query)?[0];
    }
}