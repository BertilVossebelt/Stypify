using System;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.ViewModels;

public class LessonViewModel : ViewModelBase
{
    private Lesson _lesson;
    private Exercise _exercise;

    public ICommand CheckCommand { get; set; }
    public ICommand BackButton { get; set; }
    public Lesson Lesson
    {
        get => _lesson;
        set
        {
            _lesson = value;
            OnPropertyChanged();
        }
    }
    
    public Exercise Exercise
    {
        get => _exercise;
        set
        {
            _exercise = value;
            OnPropertyChanged();
        }
    }

    public LessonViewModel(NavigationService studentDashboardViewModel, LessonStore lessonStore)
    {
        Lesson = lessonStore.CurrentLesson;
        Exercise = Lesson.Exercises[0];
        
        BackButton = new NavigateCommand(studentDashboardViewModel);
        CheckCommand = new CheckCommand();
    }
}