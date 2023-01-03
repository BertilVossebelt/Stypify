using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.ViewModels;

public class MyLessonsViewModel : ViewModelBase
{
    private List<Lesson>? _lessons;
    private List<Exercise>? _exercises;
    private Lesson? _selectedLesson;
    private Exercise? _selectedExercise;
    private NavigationService _createLessonNavigationService;
    private LessonStore _lessonStore;

    public ICommand BackButton { get; }
    public ICommand CreateExerciseButton { get; }
    public ICommand CreateLessonButton { get; }

    public List<Lesson>? Lessons
    {
        get => _lessons;
        set
        {
            if (value == null) return;
            _lessons = value;
            OnPropertyChanged();
        }
    }
    
    public List<Exercise>? Exercises
    {
        get => _exercises;
        set
        {
            if (value == null) return;
            _exercises = value;
            OnPropertyChanged();
        }
    }
    public Lesson SelectedLesson
    {
        get => _selectedLesson;
        set
        {
            _selectedLesson = value;
            _lessonStore.SetCurrentLesson(value);
            //Test if lessonstore has correct lesson:
            Console.WriteLine(_lessonStore.CurrentLesson.Name);
            new NavigateCommand(_createLessonNavigationService);
            OnPropertyChanged();
        }
    }

    public Exercise SelectedExercise
    {
        get => _selectedExercise;
        set
        {
            _selectedExercise = value;
            OnPropertyChanged();
        }
    }

    public MyLessonsViewModel(NavigationService teacherDashboardNavigationService, NavigationService createExerciseNavigationService, NavigationService createLessonNavigationService, UserStore userStore, LessonStore lessonStore)
    {
        _createLessonNavigationService = createLessonNavigationService;
        _lessonStore = lessonStore;
        BackButton = new NavigateCommand(teacherDashboardNavigationService);
        CreateExerciseButton = new NavigateCommand(createExerciseNavigationService);
        CreateLessonButton = new NavigateCommand(createLessonNavigationService);
        Exercises = new List<Exercise>();
        Lessons = new List<Lesson>();

        // Check if user is a teacher.
        if (userStore.Teacher == null) return;

        // Populate Exercises
        var exercises = new ExerciseProvider().GetAll(userStore.Teacher.Id);
        exercises?.ForEach(e => Exercises?.Add(new Exercise((string)e["text"], (string)e["name"])));

        // Populate lessons
        lessonStore.GetLessonsForTeacher();
        Lessons = lessonStore.Lessons;
    }
}