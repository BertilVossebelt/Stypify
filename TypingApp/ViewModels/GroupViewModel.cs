using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;

namespace TypingApp.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {
        public ICommand AddGroupButton { get; }
        private User _user;
        private string _boundNumber;

        private Group _SelectedItem;
        public Group SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                Console.WriteLine(SelectedItem.GroupName);
                Students.Clear();
                getStudentsFromGroup();
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
        private ObservableCollection<Group> _Groups;
        public ObservableCollection<Group> Groups
        {
            get => _Groups;
            set
            {
                _Groups = value;
                OnPropertyChanged(nameof(Groups));
            }
        }
        private ObservableCollection<Student> _Student;
        public ObservableCollection<Student> Students
        {
            get => _Student;
            set
            {
                _Student = value;
                OnPropertyChanged(nameof(Students));
            }
        }
        public GroupViewModel(TypingApp.Services.NavigationService addGroupNavigationService, User user ,DatabaseConnection connection)
        {
            _user = user;
            BoundNumber = "Naam niet gevonden";
            _connection = connection;

            var reader = _connection.ExecuteSqlStatement($"SELECT first_name, preposition, last_name FROM Users WHERE id='{_user.Id}'");
            AddGroupButton = new AddGroupCommand(connection, addGroupNavigationService);
            
            if (reader != null)
            {
                while (reader.Read())
                {
                    string preposition =" ";
                    if (!reader.IsDBNull(1))
                    {
                        preposition = reader.GetString(1) + " ";
                    }
                    else
                    {
                        preposition = "";
                    }

                    BoundNumber = ("Welkom " + reader.GetString(0) + " " + preposition + reader.GetString(2));
                }
                reader.Close();
            }
            


            Groups = new ObservableCollection<Group>();
            var reader2 = _connection.ExecuteSqlStatement($"SELECT name, id, code  FROM Groups WHERE teacher_id='{_user.Id}'" ); //TODO TESTEN

            if (reader2 != null)
            {
                var previousName = " ";
                int count = 0;
                while (reader2.Read())
                {
                    Groups.Add(new Group(reader2.GetString(0), 10, reader2.GetInt32(1), reader2.GetString(2)));
                    Console.WriteLine(reader2.GetString(0));
                    Console.WriteLine("Groep id: " + reader2.GetInt32(1));
                }
                reader2.Close();
            }
            var groupId = 4;
            Students = new ObservableCollection<Student>();  
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void getStudentsFromGroup()
        {
            Console.WriteLine(SelectedItem.Id);
            if (SelectedItem != null)
            {
                var reader3 = _connection.ExecuteSqlStatement($"SELECT Users.first_name ,Users.preposition, Users.last_name FROM Users JOIN Group_Student ON Users.id = Group_Student.student_id WHERE Group_Student.group_id='{SelectedItem.Id}'");
                if (reader3 != null)
                {
                    while (reader3.Read())
                    {
                        Console.WriteLine(reader3.GetString(0));
                        Students.Add(new Student($"{reader3.GetString(0)} {reader3.GetString(2)}", 0, 0));
                    }
                }
                else Console.WriteLine("reader = null");
                reader3.Close();
            }
        }




    }
}
