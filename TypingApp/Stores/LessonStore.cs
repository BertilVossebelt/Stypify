using System;
using System.Collections.Generic;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace TypingApp.Stores;

public class LessonStore
{
    private UserStore _userStore;
    
    public List<Lesson>? Lessons { get; private set; }
    public Lesson? CurrentLesson { get; private set; }
    public int CurrentExercise { get; private set; }
    public List<Character>? AuditedTextAsCharList { get; private set; }

    public event Action<List<Character>?>? AuditedExerciseCreated;
    public event Action<List<Lesson>?>? LessonsLoaded;
    public event Action<Lesson>? CurrentLessonUpdated;
    public Action<int>? NextExercise;

    public LessonStore(UserStore userStore)
    {
        _userStore = userStore;
    }
    
    public void LoadLessons()
    {
        Lessons = new List<Lesson>();
        List<Dictionary<string, object>>? groups = null;
        
        // Get the groups from the user that is logged in.
        if (_userStore.Student != null) groups = new StudentProvider().GetGroups(_userStore.Student.Id);
        if (_userStore.Teacher != null) groups = new TeacherProvider().GetGroups(_userStore.Teacher.Id); 
        if (groups == null) return;
        
        foreach (var group in groups)
        {
            // Get the lessons of the group.
            var lessons = new GroupProvider().GetLessons((int)group["id"]);
            //Checks if the user is a student to get the right lessons that have the completed attribute.
            if (_userStore.Student != null) lessons = new GroupProvider().GetLessonsWithCompleteAttribute((int)group["id"]);
            if (lessons == null) return;
           
            foreach (var lesson in lessons)
            {
                // Get the exercises of the lesson and add them to a list
                var exercises = new List<Exercise>();
                var result = new LessonProvider().GetExercises((int)lesson["id"]);
                result?.ForEach(r => exercises.Add(new Exercise((string)r["text"], (string)r["name"])));

                // Get the name of the teacher using a helper function.
                var teacherName = GetTeacherName((int)lesson["teacher_id"]);
                
                // Get the completed lessons if user is a student, otherwise it will create the lesson.
                if(_userStore.Student != null)
                {
                    // Gets the variable that checks if a lesson is completed or not.
                    var completed = lesson["completed"].Equals(0) || lesson["completed"].Equals(DBNull.Value) ? false : true;

                    // Finally, create the lessons.
                    Lessons.Add(new Lesson((int)lesson["id"], (string)lesson["name"], teacherName, completed, exercises));
                }
                // Finally, create the lessons.
                else Lessons.Add(new Lesson((int)lesson["id"], (string)lesson["name"], teacherName, exercises));
            }
        }
        
        LessonsLoaded?.Invoke(Lessons);
    }
    public void LoadUncompletedLessons()
    {
        Lessons = new List<Lesson>();
        List<Dictionary<string, object>>? groups = null;

        // Get the groups from the user that is logged in.
        if (_userStore.Student != null) groups = new StudentProvider().GetGroups(_userStore.Student.Id);
        if (_userStore.Teacher != null) groups = new TeacherProvider().GetGroups(_userStore.Teacher.Id);
        if (groups == null) return;

        foreach (var group in groups)
        {
            // Get the uncompleted lessons of the group.
            var lessons = new GroupProvider().GetLessonsWithCompleteAttribute ((int)group["id"]);
            if (lessons == null) return;

            foreach (var lesson in lessons)
            {
                // Get the exercises of the lesson and add them to a list
                var exercises = new List<Exercise>();
                var result = new LessonProvider().GetExercises((int)lesson["id"]);
                result?.ForEach(r => exercises.Add(new Exercise((string)r["text"], (string)r["name"])));

                // Get the name of the teacher using a helper function.
                var teacherName = GetTeacherName((int)lesson["teacher_id"]);

                // Gets the variable that checks if a lesson is completed or not.
                var completed = lesson["completed"].Equals(0) || lesson["completed"].Equals(DBNull.Value) ? false : true;

                if (!completed)
                {
                // Finally, create the lessons.
                Lessons.Add(new Lesson((int)lesson["id"], (string)lesson["name"], teacherName, completed, exercises));
                }

            }
        }

        LessonsLoaded?.Invoke(Lessons);
    }


    /*
    * ========
    * Lessons
    * ========
    */

    /*
     * Updates the current lesson.
     * ---------------------------------
     * Also sets the current exercise if
     * user is a student.
     */
    public void SetCurrentLesson(Lesson? lesson)
    {
        // Update current exercise if user is a student.
        if (_userStore.Student != null && lesson != null)
        {
            var dbLesson = new StudentProvider().GetLessonById(lesson.Id, _userStore.Student.Id);
            if (dbLesson?["place_number"] != null)
            {
                CurrentExercise = (byte)dbLesson["place_number"];
            }
            else
            {
                // Set the current exercise to 0 if the student has not started the lesson yet.
                CurrentExercise = 0;
            }
        }

        CurrentLesson = lesson;
        if (CurrentLesson != null) CurrentLessonUpdated?.Invoke(CurrentLesson);
    }

    /*
     * Updates the current exercise to the next one.
     * ------------------------------------------------
     * Resets the current exercise to 0 if student has
     * completed the lesson.
     */
    public void GoToNextExercise()
    {
        CurrentExercise = CurrentLesson != null && CurrentExercise < CurrentLesson.Exercises.Count - 1 ? CurrentExercise + 1 : 0;
        NextExercise?.Invoke(CurrentExercise);
    }

    /*
    * ==================
    * Audited exercises
    * ==================
    */
    public void CreateAuditedExercise(List<Character>? characters)
    {
        AuditedTextAsCharList = characters;
        AuditedExerciseCreated?.Invoke(AuditedTextAsCharList);
    }
    
    /*
     * =================
     * Helper functions
     * =================
     */
    private string GetTeacherName(int teacherId)
    {
        string? teacherName;
        if (_userStore.Teacher == null)
        {
            // Get the name of the teacher from the database, if user is a student.
            var teacher = new TeacherProvider().GetById(teacherId); 
            teacherName = teacher == null ? "Onbekend" : $"{(string)teacher["preposition"]} {(string)teacher["last_name"]}";
        }
        else
        {
            // If user is a teacher, just get the name from the user store.
            teacherName = $"{_userStore.Teacher.Preposition} {_userStore.Teacher.LastName}";
        }

        return teacherName;
    }
}