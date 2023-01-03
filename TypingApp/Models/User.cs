using System;
using System.Collections.Generic;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Models;

public class User
{
    public int Id { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string? Preposition { get; }
    public string LastName { get; }
    public bool IsTeacher { get; }
    public bool IsAdmin { get; }
    
    // Not in database
    public string FullName { get; }

    public User(Dictionary<string, object>? props)
    {
        Id = (int)props["id"];
        Email = (string)props["email"];
        FirstName = (string)props["first_name"];
        LastName = (string)props["last_name"];
        IsTeacher = (byte)props["teacher"] == 1;
        IsAdmin = (byte)props["admin"] == 1;

        if (props["preposition"].ToString().Length > 0) Preposition = (string)props["preposition"];
    }

    public User(int id, string email, string firstName,string? preposition, string lastName, bool isTeacher, bool isAdmin)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        Preposition = preposition;
        LastName = lastName;
        IsTeacher = isTeacher;
        IsAdmin = isAdmin;
        FullName = firstName + " " + Preposition+ " " + LastName;
    }
}