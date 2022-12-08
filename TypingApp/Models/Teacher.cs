using System.Collections.Generic;

namespace TypingApp.Models;

public class Teacher : User
{
    public Teacher(Dictionary<string, object>? props) : base(props)
    {
    }

    public Teacher(int id, string email, string firstName, string lastName, bool isTeacher, bool isAdmin) : base(id, email, firstName, lastName, isTeacher, isAdmin)
    {
    }
}