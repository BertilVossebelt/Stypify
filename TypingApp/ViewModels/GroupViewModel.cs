using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {
        private ICommand AddGroupButton { get; }
        private readonly User _user;
        private string _boundNumber;
        private Group _SelectedItem;

        private Group SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                Students.Clear();
                GetStudentsFromGroup();
                OnPropertyChanged();
            }
        }

        private string BoundNumber
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

        private ObservableCollection<Group> _groups;

        private ObservableCollection<Group> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Student> _student;

        public ObservableCollection<Student> Students
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged();
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        public GroupViewModel(NavigationService addGroupNavigationService, User user, DatabaseConnection connection)
        {
            _user = user;
            BoundNumber = "Naam niet gevonden";
            _connection = connection;

            var reader = _connection.ExecuteSqlStatement(
                    $"SELECT first_name, preposition, last_name FROM Users WHERE id='{_user.Id}'");

            AddGroupButton = new AddGroupCommand(connection, addGroupNavigationService);

            while (reader.Read())
            {
                var preposition = "";
                if (!reader.IsDBNull(1)) preposition = reader.GetString(1) + " ";
                BoundNumber = ("Welkom " + reader.GetString(0) + " " + preposition + reader.GetString(2));
            }

            reader.Close();

            Groups = new ObservableCollection<Group>();
            reader = _connection.ExecuteSqlStatement(
                $"SELECT name, id, code  FROM Groups WHERE teacher_id='{_user.Id}'"); //TODO TESTEN
            while (reader.Read())
            {
                var groupName = reader.GetString(0);
                var id = reader.GetInt32(1);
                var groupCode = reader.GetString(2);

                Groups.Add(new Group(groupName, 10, id, groupCode));
            }

            reader.Close();
            Students = new ObservableCollection<Student>();
        }

        private new void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GetStudentsFromGroup()
        {
            var reader = _connection.ExecuteSqlStatement(
                "SELECT Users.first_name ,Users.preposition, Users.last_name " +
                "FROM Users JOIN Group_Student ON Users.id = Group_Student.student_id " +
                $"WHERE Group_Student.group_id='{SelectedItem.GroupId}'");

            while (reader.Read())
            {
                Students.Add(new Student($"{reader.GetString(0)} {reader.GetString(2)}", 0, 0));
            }

            reader.Close();
        }
    }
}