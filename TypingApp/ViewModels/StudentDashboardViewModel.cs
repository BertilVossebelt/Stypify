using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using TypingApp.Models;
using TypingApp.Stores;
using Group = TypingApp.Models.Group;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using TypingApp.Views;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    private readonly UserStore _userStore;
    private ObservableCollection<Lesson> _lessons;
    private bool _isFilterChecked;
    
    public ICommand StartPracticeButton { get; }
    public ICommand AddToGroupButton { get; }
    public ICommand LogOutButton { get; }

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
    
    public StudentDashboardViewModel(UserStore userStore ,NavigationService exerciseNavigationService, NavigationService linkToGroupNavigationService, NavigationService loginNavigationService)
    {
        _userStore = userStore;

        WelcomeNameText = GetName();
        CompletedExercisesText = GetCompletedExercises();
        
        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
        AddToGroupButton = new NavigateCommand(linkToGroupNavigationService);
        LogOutButton = new LogOutCommand(userStore, loginNavigationService);

        Lessons = new ObservableCollection<Lesson>();
        //Dummy Lessons
        getLessons();
    }


    private string GetName()
    {
        if (_userStore.Student != null)
        {
            return $"Welkom {_userStore.Student.FirstName} {_userStore.Student.Preposition} {_userStore.Student.LastName}";
        }
        else return "Error, student = Null";

    }

    private string GetCompletedExercises()
    {
        return "Aantal gemaakte oefeningen: 0";
    }

    

    private void getLessons()
    {
        //TODO: get lessons from database
        Lessons.Clear(); Lessons.Add(new Lesson("Lesson","Teacher 1",1)); Lessons.Add(new Lesson("Completed lesson", "Teacher 1", 1));
        Lessons.Add(new Lesson("Lesson", "Teacher 1", 1)); Lessons.Add(new Lesson("Completed lesson", "Teacher 1", 1));
    }
    private void getNonCompletedLessons()
    {
        //TODO: get lessons that are not completed from database
        Lessons.Clear(); Lessons.Add(new Lesson("Lesson", "Teacher 1", 1));
    }

    private void FilterCompletedLessons(bool isChecked)
    {
        if (isChecked) { getNonCompletedLessons(); }
        else getLessons();
    }


}