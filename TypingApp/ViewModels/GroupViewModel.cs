using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using TypingApp.Commands;
using TypingApp.Models;

namespace TypingApp.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {

        private string _boundNumber;
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
        public GroupViewModel(DatabaseConnection connection)
        {
            BoundNumber = "Naam niet gevonden";
            _connection = connection;
            var reader = _connection.ExecuteSqlStatement("SELECT first_name, preposition, last_name FROM Users"); //TODO veranderen naar de naam van de user, want nu gaat hij overal doorheen en pakt de laatste als user
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
            Groups.Add(new Group("dads", 10, 10)); //TODO moet nog gekoppeld worden
            Groups.Add(new Group("dads", 10, 10));
            Groups.Add(new Group("dads", 10, 10));
            Groups.Add(new Group("dads", 10, 10));
            var reader2 = _connection.ExecuteSqlStatement("SELECT name, group_id  FROM Groups JOIN Group_Student ON Groups.id = Group_Student.group_id");//TODO TESTEN
            if (reader2 != null)
            {
                var previousName = " ";
                int count = 0;
                while (reader2.Read())
                {
                    if (previousName.Equals(reader2.GetString(0)) || previousName.Equals(" "))
                    {
                        count++;
                    }
                    else
                    {
                        count = 0;
                        count++;
                        Groups.Add(new Group(reader2.GetString(0), count, reader2.GetInt32(1)));
                    }
                    previousName = reader2.GetString(0);
                }
                reader2.Close();
            }
            var groupId = 4;
            Students = new ObservableCollection<Student>();
            Students.Add(new Student("dad", 10, 100)); //TODO Moet nog gekoppeld worden
            Students.Add(new Student("dad", 10, 100));
            Students.Add(new Student("dad", 10, 100));
            Students.Add(new Student("dad", 10, 100));
            var reader3 = _connection.ExecuteSqlStatement(("SELECT first_name, preposition, last_name FROM Users JOIN Group_Student ON Users.id = Group_Student.Student_id WHERE Group_student.Group_id ="+ groupId ));
            if(reader3 != null)
            {
                reader3.Close();
            }
            
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
    }
}
