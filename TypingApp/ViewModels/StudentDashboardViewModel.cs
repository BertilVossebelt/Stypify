using System;
using System.Collections.Generic;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;
using System.ComponentModel;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    private readonly UserStore _userStore;
    private List<Lesson>? _lessons;
    private bool _isFilterChecked;
    private readonly LessonStore _lessonStore;
    private Lesson _selectedLessons;

    public ICommand StartPracticeButton { get; }
    public ICommand AddToGroupButton { get; }
    public ICommand LogOutButton { get; }
    public ICommand StartLessonCommand { get; set; }
    public string WelcomeNameText { get; set; }
    public string? CompletedExercisesText { get; set; }

    public List<Lesson>? Lessons
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
            _lessonStore.SetCurrentLesson(SelectedLesson);
            StartLessonCommand.Execute(this);
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

    private string? GetCompletedExercises()
    {
        if (_userStore.Student == null) return null;
        var completedExercises = new StudentProvider().GetStudentStatistics(_userStore.Student.Id);

        return $"Aantal gemaakte oefeningen: {completedExercises?["completed_exercises"]}";
    }

    private void getNonCompletedLessons()
    {
        _lessonStore.LoadUncompletedLessons();
        Lessons = _lessonStore.Lessons;
    }

    private void FilterCompletedLessons(bool isChecked)
    {
        if (isChecked)
        {
            getNonCompletedLessons();
        }
        else
        {
            _lessonStore.LoadLessons();
            Lessons = _lessonStore.Lessons;
        }
    }
}