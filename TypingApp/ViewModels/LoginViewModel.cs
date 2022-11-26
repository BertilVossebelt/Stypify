using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.Views;

namespace TypingApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email;
        private SecureString _password;

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

        public ICommand GoToRegisterButton { get; }
        public ICommand LoginButton { get; }

        public LoginViewModel(NavigationStore navigationStore, DatabaseConnection connection)
        {
            _connection = connection;
            GoToRegisterButton = new GoToRegisterCommand(navigationStore, _connection);
            LoginButton = new LoginCommand(this, navigationStore, _connection);
        }
    }
}
