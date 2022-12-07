using System;
using System.Collections.Generic;

namespace TypingApp.Models;

public class Student : User
{
    public uint Accuracy { get; set; }
    public uint AmountOfExercises { get; set; }
    public List<Character>? Characters { get; set; }


    public Student(int id, string email, string firstName, string lastName, bool isTeacher, uint accuracy,
        uint amountOfExercises, List<Character> characters) : base(id, email, firstName, lastName, isTeacher)
    {
        Accuracy = accuracy;
        AmountOfExercises = amountOfExercises;
        Characters = characters;
    }
    
    public Student(int id, string email, string firstName, string lastName, bool isTeacher, uint accuracy,
        uint amountOfExercises) : base(id, email, firstName, lastName, isTeacher)
    {
        Accuracy = accuracy;
        AmountOfExercises = amountOfExercises;
    }
    
    // TODO: Accuracy, AmountOfExercises and Characters should be queried from database with a StudentProvider.
    public Student(IReadOnlyList<Dictionary<string, object>>? props, List<Character> characters) : base(props)
    {
        Accuracy = 5;
        AmountOfExercises = 5;
        Characters = characters;
    }
}