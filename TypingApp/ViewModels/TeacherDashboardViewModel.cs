using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

public class TeacherDashboardViewModel : ViewModelBase
{
    private readonly UserStore _userStore;
    private string _welcomeMessage;
    private Group _selectedItem;
    private ObservableCollection<Group> _groups;
    private ObservableCollection<Student> _students;
    private int _amountOfExercises;

    public ICommand AddGroupButton { get; }
    public ICommand MyLessonsButton { get; }
    public ICommand LogOutButton { get; }

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
    
    public int AmountOfExercises
    {
        get => _amountOfExercises;
        set
        {
            _amountOfExercises = value;
            OnPropertyChanged();
        }
    }


    public TeacherDashboardViewModel(NavigationService addGroupNavigationService,
        NavigationService myLessonsNavigationService, NavigationService loginNavigationService, UserStore userStore)
    {
        Console.WriteLine("TeacherDashboardViewModel");
        _userStore = userStore;

        MyLessonsButton = new NavigateCommand(myLessonsNavigationService);
        AddGroupButton = new NavigateCommand(addGroupNavigationService);
        LogOutButton = new LogOutCommand(userStore, loginNavigationService);
        UpdateGroupsCodeButton = new UpdateGroupsCodeCommand(this);
        
        Groups = new ObservableCollection<Group>();
        Students = new ObservableCollection<Student>();

        if (_userStore.Teacher == null) return;
        WelcomeMessage = $"Welkom {_userStore.Teacher.FirstName} " +
                         $"{_userStore.Teacher.Preposition} " +
                         $"{_userStore.Teacher.LastName}";
        var groups = new TeacherProvider().GetGroups(_userStore.Teacher.Id);
        
        if (groups == null) return;
        foreach (var group in groups)
        {
            Groups.Add(new Group(group));
        }

        Console.WriteLine("aaaaaaaaaaaaaaaaaa");
    }

    private void GetStudentsFromGroup()
    {
        var students = new GroupProvider().GetStudents(SelectedItem.GroupId);

        if (students == null) return;
        // TODO: Should be queried from database.
        var characters = new List<Character>
        {
            new('e'),
            new('n'),
            new('a'),
            new('t'),
        };

        foreach (var student in students)
        {
            Students.Add(new Student(student, characters));
        }

        _selectedItem.AmountOfStudents = students.Count;
        Students = Students;
    }
}