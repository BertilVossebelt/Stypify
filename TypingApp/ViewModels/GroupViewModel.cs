using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TypingApp.Commands;

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
        public GroupViewModel(DatabaseConnection connection)
        {
            BoundNumber = "Naam niet gevonden";
            _connection = connection;
            var reader = _connection.ExecuteSqlStatement("SELECT first_name, preposition, last_name FROM Users"); //TODO veranderen naar de naam van de user
            while (reader.Read())
            {
                BoundNumber = ("Welkom "+reader.GetString(0) + " " + reader.GetString(1) + " " + reader.GetString(2));
            }
            
            
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
