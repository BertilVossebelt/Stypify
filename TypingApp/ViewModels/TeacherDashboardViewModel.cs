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
    private string _boundNumber;
    private Group _selectedItem;
    private ObservableCollection<Group> _groups;
    private ObservableCollection<Student> _student;

    public Group SelectedItem
    {
        get { return _selectedItem; }
        set
        {
            _selectedItem = value;
            Students.Clear();
            GetStudentsFromGroup();
            OnPropertyChanged();
        }
    }

    public string BoundNumber
    {
        get { return _boundNumber; }
        set
        {
            if (_boundNumber != value)
            {
                _boundNumber = value;
                OnPropertyChanged();
            }
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
        get => _student;
        set
        {
            _student = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddGroupButton { get; }
    public new event PropertyChangedEventHandler PropertyChanged;

    public TeacherDashboardViewModel(NavigationService addGroupNavigationService, UserStore userStore,
        DatabaseService connection)
    {
        BoundNumber = "Naam niet gevonden";
        _connection = connection;

        var reader = _connection.ExecuteSqlStatement(
            $"SELECT first_name, preposition, last_name FROM Users WHERE id='{userStore.User.Id}'");
        AddGroupButton = new AddGroupCommand(connection, addGroupNavigationService);

        while (reader.Read())
        {
            var preposition = " ";
            if (!reader.IsDBNull(1)) preposition = reader.GetString(1) + " ";
            BoundNumber = ("Welkom " + reader.GetString(0) + " " + preposition + reader.GetString(2));
        }

        reader.Close();

        Groups = new ObservableCollection<Group>();
        reader = _connection.ExecuteSqlStatement(
            $"SELECT name, id, code  FROM Groups WHERE teacher_id='{userStore.User.Id}'"); //TODO TESTEN
        {
            while (reader.Read())
            {
                var groupName = reader.GetString(0);
                var id = reader.GetInt32(1);
                var groupCode = reader.GetString(2);
                
                Groups.Add(new Group(groupName, 10, id, groupCode));
            }

            reader.Close();
        }
        Students = new ObservableCollection<Student>();
    }

    private new void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void GetStudentsFromGroup()
    {
        var student = new GroupProvider().GetStudents(SelectedItem.GroupId);
            
        var characters = new List<Character>()
        {
            new('e'),
            new('n'),
            new('a'),
            new('t'),
        };

        new Student(student, characters);
    }
}