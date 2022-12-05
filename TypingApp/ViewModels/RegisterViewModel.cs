﻿using System.Security;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;

namespace TypingApp.ViewModels
{
    public class RegisterViewModel : ViewModelBase
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public ICommand GoToLoginButton { get; }
        public ICommand RegisterButton { get; }

        public RegisterViewModel(NavigationService loginNavigationService, DatabaseConnection connection) 
        {
            _connection = connection;
            GoToLoginButton = new NavigateCommand(loginNavigationService);
            RegisterButton = new RegisterStudentCommand(this, connection);
        }
    }
}
