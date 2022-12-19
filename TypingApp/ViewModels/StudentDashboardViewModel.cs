using System;
using System.Collections.Generic;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;
using System.Collections.ObjectModel;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    private readonly UserStore _userStore;
    private ObservableCollection<Lesson> _lessons;
    private bool _isFilterChecked;

    public ICommand StartPracticeButton { get; }
    public ICommand AddToGroupButton { get; }
    public ICommand LogOutButton { get; }
    public ICommand LessonClickCommand { get; set; }
    public string WelcomeNameText { get; set; }
    public string CompletedExercisesText { get; set; }

    public ObservableCollection<Lesson> Lessons
    {
        get => _lessons;
        set
        {
            _lessons = value;
            OnPropertyChanged();
        }
    }

    public bool IsFilterChecked
    {
        get => _isFilterChecked;
        set
        {
            _isFilterChecked = value;
            FilterCompletedLessons(IsFilterChecked);
            OnPropertyChanged();
        }
    }

    public StudentDashboardViewModel(UserStore userStore, NavigationService exerciseNavigationService,
        NavigationService linkToGroupNavigationService, NavigationService loginNavigationService,
        NavigationService customExerciseNavigationService)
    {
        _userStore = userStore;

        WelcomeNameText = GetName();
        CompletedExercisesText = GetCompletedExercises();

        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
        AddToGroupButton = new NavigateCommand(linkToGroupNavigationService);
        LessonClickCommand = new NavigateCommand(customExerciseNavigationService);
        LogOutButton = new LogOutCommand(userStore, loginNavigationService);

        Lessons = new ObservableCollection<Lesson>();

        GetLessons();
    }


    private string GetName()
    {
        if (_userStore.Student?.Preposition != null)
        {
            return
                $"Welkom {_userStore.Student.FirstName} {_userStore.Student.Preposition} {_userStore.Student.LastName}";
        }

        if (_userStore.Student?.Preposition == null)
        {
            return $"Welkom {_userStore.Student?.FirstName} {_userStore.Student?.LastName}";
        }

        return "Error, student = Null";
    }

    private string GetCompletedExercises()
    {
        return "Aantal gemaakte oefeningen: 0";
    }

    // TODO: Save lessons in store so it doesn't have to be fetched every time.
    private void GetLessons()
    {
        // Check if user is a student.
        if (_userStore.Student == null) return;
        Lessons.Clear();

        // Get the groups of the student.
        var groups = new StudentProvider().GetGroups(_userStore.Student.Id);

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
                var result = new ExerciseProvider().GetAll((int)lesson["id"]);
                result?.ForEach(r => exercises.Add(new Exercise((string)r["text"], (string)r["name"])));

                // Finally, create the lessons.
                Lessons.Add(new Lesson((int)lesson["id"], (string)lesson["name"], "Naam", exercises));
            }
        }
    }

    private void getNonCompletedLessons()
    {
        //TODO: get lessons that are not completed from database
        Lessons.Clear();
        // Lessons.Add(new Lesson(1, "Lesson", "Teacher 1"));
    }

    private void FilterCompletedLessons(bool isChecked)
    {
        if (isChecked)
        {
            getNonCompletedLessons();
        }
        else GetLessons();
    }
}