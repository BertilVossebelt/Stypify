using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

    public ICommand AddGroupButton { get; }
    public new event PropertyChangedEventHandler PropertyChanged;

    public TeacherDashboardViewModel(NavigationService addGroupNavigationService, UserStore userStore)
    {
        _userStore = userStore;

        AddGroupButton = new NavigateCommand(addGroupNavigationService);
        Groups = new ObservableCollection<Group>();
        Students = new ObservableCollection<Student>();
        Groups.Add(new Group("DummyGroep", 2, 1, "test"));


        if (_userStore.Teacher != null)
        {
            WelcomeMessage = $"Welkom {_userStore.Teacher.FirstName}" +
                             $" {_userStore.Teacher.Preposition}" +
                             $" {_userStore.Teacher.LastName}";

            var teacherProvider = new TeacherProvider();
            var groups = teacherProvider.GetGroups(_userStore.Teacher.Id);
            
            if (groups != null)
            {
                foreach (var group in groups) Groups.Add(new Group(group));
            }
        }
    }

    private new void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void GetStudentsFromGroup()
    {
        var students = new GroupProvider().GetStudents(SelectedItem.GroupId);
        if (students == null) return;

        // TODO: Should be queried from database instead
        var characters = new List<Character>()
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
    }
}