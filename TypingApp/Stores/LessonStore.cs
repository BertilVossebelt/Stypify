using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Stores;

public class LessonStore
{
    private UserStore _userStore;

    public LessonStore(UserStore userStore)
    {
        _userStore = userStore;
    }

    public event Action<List<Lesson>>? LessonsLoaded;

    public event Action<Lesson>? CurrentLessonUpdated;
    public Action<int>? NextExercise;

    public List<Lesson> Lessons { get; private set; }

    public Lesson CurrentLesson { get; private set; }

    public int CurrentExercise { get; private set; }

    public List<Character> AuditedTextAsCharList { get; private set; }

    public event Action<List<Character>>? AuditedExerciseCreated;

    public void LoadLessons(UserStore userStore)
    {
        GetLessonsForStudent();
        GetLessonsForTeacher();
    }

    public void GetLessonsForStudent()
    {
        if (_userStore.Student == null) return;
        Lessons = new List<Lesson>();

        // Get the groups of the student.
        var groups = new StudentProvider().GetGroups(_userStore.Student.Id);

        if (groups == null) return;

        foreach (var group in groups)
        {
            // Get the lessons of the group.
            var lessons = new GroupProvider().GetLessons((int)group["id"]);

            if (lessons == null) return;
            foreach (var lesson in lessons)
            {
                // Get the exercises of the lesson.
                var exercises = new List<Exercise>();
                var result = new LessonProvider().GetExercises((int)lesson["id"]);
                
                
                result?.ForEach(r => exercises.Add(new Exercise((string)r["text"], (string)r["name"])));

                // Get the name of the teacher.
                var teacher = new TeacherProvider().GetById((int)lesson["teacher_id"]);
                var teacherName = teacher == null
                    ? "Onbekend"
                    : $"{(string)teacher["preposition"]} {(string)teacher["last_name"]}";
                
                // Finally, create the lessons.
                Lessons.Add(new Lesson((int)lesson["id"], (string)lesson["name"], teacherName, exercises));
            }
            LessonsLoaded?.Invoke(Lessons);
        }
    }
    
    public void GetLessonsForTeacher()
    {
        if (_userStore.Teacher == null) return;
        Lessons = new List<Lesson>();
        
        // Get the groups of the teacher.
        var groups = new TeacherProvider().GetGroups(_userStore.Teacher.Id);

        if (groups == null) return;
        foreach (var group in groups)
        {
            // Get the lessons of the group.
            var lessons = new GroupProvider().GetLessons((int)group["id"]);
            if (lessons == null) return;
            foreach (var lesson in lessons)
            {
                // Get the exercises of the lesson.
                var exercises = new List<Exercise>();
                var result = new LessonProvider().GetExercises((int)lesson["id"]);
                result?.ForEach(r => exercises.Add(new Exercise((string)r["text"], (string)r["name"])));
                
                // Get the name of the teacher.
                var name = $"{_userStore.Teacher?.Preposition} {_userStore.Teacher?.LastName}";

                // Finally, create the lessons.
                Lessons.Add(new Lesson(_userStore.Teacher!.Id, (string)lesson["name"], name, exercises));
            }
            LessonsLoaded?.Invoke(Lessons);
        }
    }

    /*
    * ========
    * Lessons
    * ========
    */
    public void SetCurrentLesson(Lesson lesson)
    {
        if (_userStore.Student == null) return;

        var dbLesson = new StudentProvider().GetLessonById(lesson.Id, _userStore.Student.Id);
        if (dbLesson?["place_number"] != null)
        {
            CurrentExercise = (byte)dbLesson["place_number"];
        }
        else
        {
            CurrentExercise = 0;
        }
        
        CurrentLesson = lesson;

        CurrentLessonUpdated?.Invoke(CurrentLesson);
    }

    public void GoToNextExercise()
    {
        CurrentExercise = CurrentExercise < CurrentLesson.Exercises.Count - 1 ? CurrentExercise + 1 : 0;
        NextExercise?.Invoke(CurrentExercise);
    }

    /*
    * ==================
    * Audited exercises
    * ==================
    */
    public void CreateAuditedExercise(List<Character> characters)
    {
        AuditedTextAsCharList = characters;
        AuditedExerciseCreated?.Invoke(AuditedTextAsCharList);
    }
}