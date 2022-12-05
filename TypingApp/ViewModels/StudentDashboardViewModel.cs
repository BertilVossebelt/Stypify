using System.Windows.Input;
using TypingApp.Models;
using TypingApp.Commands;
using TypingApp.Services;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Group = TypingApp.Models.Group;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    private ObservableCollection<Group> _Lessons;

    public ICommand StartPracticeButton { get; }
    public ICommand AddToGroupButton { get; }
    public ICommand LogOutButton { get; }

    public string WelcomeNameText { get; set; }
    public ObservableCollection<Group> Lessons
    {
        get => _Lessons;
        set
        {
            _Lessons = value;
            OnPropertyChanged();
        }
    }
    
    public StudentDashboardViewModel( User user ,NavigationService exerciseNavigationService, NavigationService linkToGroupNavigationService, NavigationService loginNavigationService)
    {
        if (user.Preposition != null) { WelcomeNameText = $"Welkom {user.FirstName} {user.Preposition} {user.LastName}"; }
        else WelcomeNameText = $"Welkom {user.FirstName} {user.LastName}";

        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
        AddToGroupButton = new NavigateCommand(linkToGroupNavigationService);
        LogOutButton = new NavigateCommand(loginNavigationService);

        Lessons = new ObservableCollection<Group>();
        Lessons.Add(new Group("TestGroup1", 10, 1, "TeStCoDe"));
        
    }


}