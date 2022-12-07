using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using TypingApp.Models;
using TypingApp.Stores;
using Group = TypingApp.Models.Group;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    private readonly UserStore _userStore;
    private DatabaseService _connection;
    private ObservableCollection<Group> _Lessons;

    public ICommand StartPracticeButton { get; }
    public ICommand AddToGroupButton { get; }
    public ICommand LogOutButton { get; }

    public string WelcomeNameText { get; set; }
    public string CompletedExercisesText { get; set; }
    public ObservableCollection<Group> Lessons
    {
        get => _Lessons;
        set
        {
            _Lessons = value;
            OnPropertyChanged();
        }
    }
    
    public StudentDashboardViewModel(UserStore userStore, DatabaseService connection ,NavigationService exerciseNavigationService, NavigationService linkToGroupNavigationService, NavigationService loginNavigationService)
    {
        _userStore = userStore;
        _connection = connection;

        WelcomeNameText = GetName();
        CompletedExercisesText = GetCompletedExercises();
        
        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
        AddToGroupButton = new NavigateCommand(linkToGroupNavigationService);
        LogOutButton = new NavigateCommand(loginNavigationService);

        Lessons = new ObservableCollection<Group>();
        Lessons.Add(new Group("TestGroup1", 10, 1, "TeStCoDe"));
        
    }


    private string GetName()
    {
        return  "Welkom " + _userStore.Student.FirstName + " " + _userStore.Student.Preposition + _userStore.Student.LastName;
    }

    private string GetCompletedExercises()
    {
        return "Aantal gemaakte oefeningen: 0";
    }


}