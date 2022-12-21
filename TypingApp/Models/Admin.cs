using System.Collections.Generic;

namespace TypingApp.Models;

public class Admin : User
{
    public Admin(Dictionary<string, object>? props) : base(props)
    {
    }

    public Admin(int id, string email, string firstName, string preposition, string lastName, bool isTeacher, bool isAdmin) : base(id, email, firstName, preposition, lastName, isTeacher, isAdmin)
    {
    }
}