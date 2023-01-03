using System;
using System.Collections.Generic;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;
using System.ComponentModel;
using TypingApp.Models;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    private readonly UserStore _userStore;
    private List<Lesson> _lessons;
    private bool _isFilterChecked;
    private readonly LessonStore _lessonStore;
    private Lesson _selectedLessons;

    public ICommand StartPracticeButton { get; }
    public ICommand AddToGroupButton { get; }
    public ICommand LogOutButton { get; }
    public ICommand StartLessonCommand { get; set; }
    public string WelcomeNameText { get; set; }
    public string CompletedExercisesText { get; set; }

    public List<Lesson> Lessons
    {
        get => _lessons;
        set
        {
            _lessons = value;
            OnPropertyChanged();
        }
    }

    public Lesson SelectedLesson
    {
        get => _selectedLessons;
        set
        {
            _selectedLessons = value;
            OnPropertyChanged(nameof(_selectedLessons));
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

    public StudentDashboardViewModel(UserStore userStore, LessonStore lessonStore,
        NavigationService exerciseNavigationService,
        NavigationService linkToGroupNavigationService, NavigationService loginNavigationService,
        NavigationService lessonNavigationService)
    {
        _userStore = userStore;
        _lessonStore = lessonStore;

        WelcomeNameText = GetName();
        CompletedExercisesText = GetCompletedExercises();
        Lessons = _lessonStore.Lessons; 

        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
        StartLessonCommand = new NavigateCommand(lessonNavigationService);
        AddToGroupButton = new NavigateCommand(linkToGroupNavigationService); 
        LogOutButton = new LogOutCommand(userStore, loginNavigationService);

        PropertyChanged -= SelectLesson;
        PropertyChanged += SelectLesson;
    }
    
    private void SelectLesson(object? sender, PropertyChangedEventArgs e)
    {
        _lessonStore.SetCurrentLesson(SelectedLesson);
        StartLessonCommand.Execute(this);
    }
    
    private string GetName()
    {
        if (_userStore.Student?.Preposition != null)
        {
            return $"Welkom {_userStore.Student.FirstName} {_userStore.Student.Preposition} {_userStore.Student.LastName}";
        }

        return _userStore.Student?.Preposition == null
            ? $"Welkom {_userStore.Student?.FirstName} {_userStore.Student?.LastName}"
            : "Error, student = Null";
    }

    private string GetCompletedExercises()
    {
        return "Aantal gemaakte oefeningen: 0";
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
        else
        {
            Lessons = _lessonStore.Lessons;
        }
    }
}