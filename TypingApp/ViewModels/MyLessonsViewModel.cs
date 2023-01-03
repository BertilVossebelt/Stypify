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
    private ObservableCollection<Lesson>? _lessons;
    private List<Exercise>? _exercises;
    private Lesson? _selectedLesson;
    private Exercise? _selectedExercise;
    private NavigationService _createLessonNavigationService;
    private LessonStore _lessonStore;

    public ICommand BackButton { get; }
    public ICommand CreateExerciseButton { get; }
    public ICommand CreateLessonButton { get; }

    public ObservableCollection<Lesson>? Lessons
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
            _lessonStore.UpdateLesson(value);
            //Test if lessonstore has correct lesson:
            Console.WriteLine(_lessonStore.Lesson.LessonName);
            var navigateCommand = new NavigateCommand(_createLessonNavigationService);
            //navigateCommand.Execute(this);
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
        Exercises = new ObservableCollection<Exercise>();
        Lessons = new ObservableCollection<Lesson>();

        // Populate Exercises
        if (userStore.Teacher == null) return;
        var exercises = new ExerciseProvider().GetAll(userStore.Teacher.Id);
        exercises?.ForEach(e => Exercises?.Add(new Exercise((string)e["text"], (string)e["name"])));

        //Populate lessons
        if (userStore.Teacher == null) return;
        var lessons = new LessonProvider().GetAll(userStore.Teacher.Id);
        lessons?.ForEach(e => Lessons?.Add(new Lesson((string)e["name"],userStore.Teacher.FullName,(int)e["id"])));

        Exercises.Add(new Exercise("testsdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
        Exercises.Add(new Exercise("test", "test"));
    }
}