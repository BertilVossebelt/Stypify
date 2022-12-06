using System;
using System.Collections.Generic;
using TypingApp.Services.Database;

namespace TypingApp.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; }
    public string FirstName { get; set; }
    public string? Preposition { get; set; }
    public string LastName { get; set; }
    public List<Character>? Characters { get; set; }
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

    public User(int id, List<Character> characters)
    {
        var user = new UserProvider().GetUserById(id);

        Id = id;
        Email = (string)user[0]["email"];
        FirstName = (string)user[0]["first_name"];
        Preposition = (string)user[0]["preposition"];
        LastName = (string)user[0]["last_name"];
        IsTeacher = (byte)user[0]["teacher"] == 1;
        Characters = characters;
    }
}