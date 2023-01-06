using System.Collections.Generic;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Models;

public class Student : User
{
    public int Accuracy { get; set; }
    public int CompletedExercises { get; set; }
    public List<Character>? Characters { get; set; }

    // TODO: Accuracy and Characters should be queried from database with a StudentProvider.
    public Student(Dictionary<string, object>? props, List<Character> characters) : base(props)
    {
        Accuracy = 5;
        CompletedExercises = 0;
        Characters = characters;
        
        var studentStatistics = new StudentProvider().GetStudentStatistics((int)props["id"]);
        CompletedExercises = (int)(studentStatistics?["completed_exercises"] ?? 0);
    }
}