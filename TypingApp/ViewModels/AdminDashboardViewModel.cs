using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Stores;

namespace TypingApp.ViewModels
{
    public class AdminDashboardViewModel : ViewModelBase
    {
        private string _email;
        private string _firstName;
        private string? _preposition;
        private string _lastName;
        private SecureString _password;
        private SecureString _passwordConfirm;

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string? Preposition
        {
            get
            {
                return _preposition;
            }
            set
            {
                _preposition = value;
                OnPropertyChanged(nameof(Preposition));
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public SecureString PasswordConfirm
        {
            get
            {
                return _passwordConfirm;
            }
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged(nameof(PasswordConfirm));
            }
        }

        public ICommand RegisterTeacherButton;

        public AdminDashboardViewModel(NavigationStore navigationStore, DatabaseConnection connection)
        {
            _connection = connection;
            // RegisterTeacherButton = new RegisterTeacherCommand();
        }
    }
}
