using System;
using System.Collections.Generic;

namespace TypingApp.Models;

public class Student : User
{
    public uint Accuracy { get; set; }
    public uint AmountOfExercises { get; set; }
    public List<Character>? Characters { get; set; }

    // TODO: Accuracy, AmountOfExercises and Characters should be queried from database with a StudentProvider.
    public Student(Dictionary<string, object>? props, List<Character> characters) : base(props)
    {
        Accuracy = 5;
        AmountOfExercises = 5;
        Characters = characters;
    }

    public Student(int id, string email, string firstName, string preposition, string lastName, bool isAdmin, bool isTeacher, uint accuracy,
        uint amountOfExercises, List<Character> characters) : base(id, email, firstName, preposition, lastName, isTeacher, isAdmin)
    {
        Accuracy = accuracy;
        AmountOfExercises = amountOfExercises;
        Characters = characters;
    }
    
    public Student(int id, string email, string firstName, string preposition, string lastName, bool isTeacher, bool isAdmin, uint accuracy,
        uint amountOfExercises) : base(id, email, firstName, preposition, lastName, isTeacher, isAdmin)
    {
        Accuracy = accuracy;
        AmountOfExercises = amountOfExercises;
    }
    
}