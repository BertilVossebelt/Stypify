using System;
using System.Collections.Generic;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Stores;

public class LessonStore
{
    public event Action<List<Lesson>>? LessonsLoaded;
    
    public event Action<Lesson>? CurrentLessonUpdated;

    public List<Lesson> Lessons { get; private set; }
    
    public Lesson CurrentLesson { get; private set; }
    
    public void LoadLessons(UserStore userStore)
    {
        // Check if user is a student.
        if (userStore.Student == null) return;
        Lessons = new List<Lesson>();

        // Get the groups of the student.
        var groups = new StudentProvider().GetGroups(userStore.Student.Id);

        if (groups == null) return;

        foreach (var group in groups)
        {
            // Get the lessons of the group.
            var lessons = new GroupProvider().GetLessons((int)group["id"]);

            if (lessons == null) continue;
            foreach (var lesson in lessons)
            {
                // Get the exercises of the lesson.
                var exercises = new List<Exercise>();
                var result = new LessonProvider().GetExercises((int)lesson["id"]);
                result?.ForEach(r => exercises.Add(new Exercise((string)r["text"], (string)r["name"])));
                
                // Get the name of the teacher.
                var teacher = new TeacherProvider().GetById((int)lesson["teacher_id"]);
                var teacherName = teacher == null ? "Unknown" : $"{(string)teacher["preposition"]} {(string)teacher["last_name"]}";
                
                // Finally, create the lessons.
                Lessons.Add(new Lesson((int)lesson["id"], (string)lesson["name"], teacherName, exercises));
            }
        }
        
        OnLessonsLoaded();     
    }

    
    public void SetCurrentLesson(Lesson lesson)
    {
        CurrentLesson = lesson;
        OnCurrentLessonUpdated();
    }

    private void OnCurrentLessonUpdated()
    {
        CurrentLessonUpdated?.Invoke(CurrentLesson);
    }

    private void OnLessonsLoaded()
    {
        LessonsLoaded?.Invoke(Lessons);
    }
}