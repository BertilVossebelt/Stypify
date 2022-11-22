using System.Collections.Generic;

namespace TypingApp.Models;

public class User
{
    public uint Id { get; }
    public string Email { get; }
    public string FirstName { get; set; }
    public string? Preposition { get; set; }
    public string LastName { get; set; }
    public List<Character> Characters { get; set; }

    public User(uint id, string email, string firstName, string lastName, List<Character> characters)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Characters = characters;
    }
}