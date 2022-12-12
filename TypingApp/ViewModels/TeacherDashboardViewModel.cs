using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.ViewModels;

public class TeacherDashboardViewModel : ViewModelBase , INotifyPropertyChanged
{
    private readonly UserStore _userStore;
    private string _welcomeMessage;
    private Group _selectedItem;
    private ObservableCollection<Group> _groups;
    private ObservableCollection<Student> _students;

    public ICommand AddGroupButton { get; }
    public ICommand MyLessonsButton { get; }
    public ICommand LogOutButton { get;  }

    public ICommand UpdateGroupsCodeButton { get; set; }




    public Group SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            
            Students.Clear();
            GetStudentsFromGroup();
            OnPropertyChanged();
         
        }
    }

    public string WelcomeMessage
    {
        get => _welcomeMessage;
        set
        {
            if (_welcomeMessage == value) return;
            _welcomeMessage = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Group> Groups
    {
        get => _groups;
        set
        {
            _groups = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Student> Students
    {
        get => _students;
        set
        {
            _students = value;
            OnPropertyChanged();
        }
    }

    public new event PropertyChangedEventHandler PropertyChanged;

    public TeacherDashboardViewModel(NavigationService addGroupNavigationService, NavigationService myLessonsNavigationService, NavigationService loginNavigationService, UserStore userStore)
    {
        _userStore = userStore;

        MyLessonsButton = new NavigateCommand(myLessonsNavigationService);
        AddGroupButton = new NavigateCommand(addGroupNavigationService);
        LogOutButton = new LogOutCommand(userStore, loginNavigationService);
        UpdateGroupsCodeButton = new UpdateGroupsCodeCommand(this);

        Groups = new ObservableCollection<Group>();
        Students = new ObservableCollection<Student>();
        Students.Add(new Student(1, "test", "test", "'n", "test", false, false, 10));
        Groups.Add(new Group(1, "DummyGroep", "ASDASD"));

        if (_userStore.Teacher != null)
        {
            WelcomeMessage = $"Welkom {_userStore.Teacher.FirstName} {_userStore.Teacher.Preposition} {_userStore.Teacher.LastName}";

            var teacherProvider = new TeacherProvider();
            var groupProvider = new GroupProvider();

            var groups = teacherProvider.GetGroups(_userStore.Teacher.Id);
            
            //var groupProvider = new GroupProvider();
            // var groups = groupProvider.GetGroupsV2(_userStore.Teacher.Id);


            if (groups != null)
            {
                foreach (var group in groups) 
                {
                    Groups.Add(new Group(group)); 
                } //

            }


        }
    }

    private new void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void GetStudentsFromGroup()
    {
        var tupleResult = new GroupProvider().GetStudents(SelectedItem.GroupId);
        Students = tupleResult.Item1;
        _selectedItem.AmountOfStudents = tupleResult.Item2;
        if (Students == null) return;

        // TODO: Should be queried from database instead
        var characters = new List<Character>()
        {
            new('e'),
            new('n'),
            new('a'),
            new('t'),
        };
     
        Students = Students;
    } 
}