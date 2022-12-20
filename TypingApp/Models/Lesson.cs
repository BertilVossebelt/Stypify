using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TypingApp.Models;

public class Lesson
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string TeacherName { get; set; }

    public List<Exercise> Exercises { get; set; }

    public Lesson(int id, string name, string teacherName, List<Exercise> exercises)
    {
        Id = id;
        Name = name;
        TeacherName = teacherName;
        Exercises = exercises;
    }
}