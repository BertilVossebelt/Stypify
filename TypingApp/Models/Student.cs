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
}