using System.Windows.Input;
using TypingApp.Models;
using TypingApp.Commands;
using TypingApp.Services;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Group = TypingApp.Models.Group;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    private User _user;
    private DatabaseConnection _connection;
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
    
    public StudentDashboardViewModel(User user, DatabaseConnection connection ,NavigationService exerciseNavigationService, NavigationService linkToGroupNavigationService, NavigationService loginNavigationService)
    {
        _user = user;
        _connection = connection;
        getName();
        
        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
        AddToGroupButton = new NavigateCommand(linkToGroupNavigationService);
        LogOutButton = new NavigateCommand(loginNavigationService);

        Lessons = new ObservableCollection<Group>();
        Lessons.Add(new Group("TestGroup1", 10, 1, "TeStCoDe"));
        
    }


    private void getName()
    {
        var reader = _connection.ExecuteSqlStatement($"SELECT first_name, preposition, last_name FROM Users WHERE id='{_user.Id}'");
        if (reader != null)
        {
            while (reader.Read())
            {
                string preposition = " ";
                if (!reader.IsDBNull(1))
                {
                    preposition = reader.GetString(1) + " ";
                }
                else
                {
                    preposition = "";
                }

                WelcomeNameText = ("Welkom " + reader.GetString(0) + " " + preposition + reader.GetString(2));
            }
            reader.Close();
        }
    }


}