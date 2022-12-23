using System.Collections.Generic;

namespace TypingApp.Models;

public class Student : User
{
    public int Accuracy { get; set; }
    public int AmountOfExercises { get; set; }
    public List<Character>? Characters { get; set; }

    // TODO: Accuracy, AmountOfExercises and Characters should be queried from database with a StudentProvider.
    public Student(Dictionary<string, object>? props, List<Character> characters) : base(props)
    {
        Accuracy = 5;
        AmountOfExercises = 0;
        Characters = characters;

        AmountOfExercises = (bool)props?.ContainsKey("AmountOfExercises") ? (int)props["AmountOfExercises"] : 0;
    }

    public Student(int id, string email, string firstName, string preposition, string lastName, bool isAdmin,
        bool isTeacher, int accuracy,
        int amountOfExercises, List<Character> characters) : base(id, email, firstName, preposition, lastName,
        isTeacher, isAdmin)
    {
        Accuracy = accuracy;
        AmountOfExercises = amountOfExercises;
        Characters = characters;
    }

    public Student(int id, string email, string firstName, string preposition, string lastName, bool isTeacher,
        bool isAdmin, int accuracy,
        int amountOfExercises) : base(id, email, firstName, preposition, lastName, isTeacher, isAdmin)
    {
        Accuracy = accuracy;
        AmountOfExercises = amountOfExercises;
    }

    public Student(int id, string email, string firstName, string preposition, string lastName, bool isTeacher,
        bool isAdmin,
        int amountOfExercises) : base(id, email, firstName, preposition, lastName, isTeacher, isAdmin)
    {
        AmountOfExercises = amountOfExercises;
    }
}