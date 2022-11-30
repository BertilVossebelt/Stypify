using System.Collections.Generic;

namespace TypingApp.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; }
    public string FirstName { get; set; }
    public string? Preposition { get; set; }
    public string LastName { get; set; }
    public List<Character> Characters { get; set; }
    public bool IsTeacher { get; set; }

    public User(int id, string email, string firstName, string lastName, List<Character> characters, bool isTeacher)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Characters = characters;
        IsTeacher = isTeacher;
    }
}