using System;
using System.Collections.Generic;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; }
    public string FirstName { get; set; }
    public string? Preposition { get; set; }
    public string LastName { get; set; }
    public bool IsTeacher { get; set; }

    public User(int id, string email, string firstName, string lastName, bool isTeacher)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        IsTeacher = isTeacher;
    }

    public User(IReadOnlyList<Dictionary<string, object>>? props)
    {
        Id = (int)props[0]["id"];
        Email = (string)props[0]["email"];
        FirstName = (string)props[0]["first_name"];
        LastName = (string)props[0]["last_name"];
        IsTeacher = (byte)props[0]["teacher"] == 1;
        
        if (props[0]["preposition"].ToString().Length > 0)
        {
            Preposition = (string)props[0]["preposition"];
        }        

    }
}