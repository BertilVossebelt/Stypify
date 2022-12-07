using System.Collections.Generic;

namespace TypingApp.Services.DatabaseProviders;

public class GroupProvider : BaseProvider
{
    public override IEnumerable<Dictionary<string, object>>? GetById(int id)
    {
        throw new System.NotImplementedException();
    }
    
    public List<Dictionary<string, object>>? GetStudents(int groupId)
    {
        var query = $"SELECT * FROM Users JOIN Group_Student ON Users.id = Group_Student.student_id WHERE Group_Student.group_id='{groupId}'";
        return DbConnection?.Select(query);
    }
}