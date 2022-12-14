using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypingApp.Models;

namespace TypingApp.Stores;

public class LessonStore
{
    public event Action<Lesson>? LessonUpdated;
    public event Action<Lesson>? LessonDeleted;
    public Lesson? Lesson { get; private set; }


    public void UpdateLesson(Lesson lesson)
    {
        Lesson = lesson;
        OnLessonUpdated();
    }

    public void DeleteLesson()
    {
        Lesson = null;
        OnLessonDeleted();
    }

    private void OnLessonUpdated()
    {
        if (Lesson != null) LessonUpdated?.Invoke(Lesson);
    }

    private void OnLessonDeleted()
    {
        if (Lesson != null) LessonDeleted?.Invoke(Lesson);
    }


}