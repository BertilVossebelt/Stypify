using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;

namespace TypingApp.ViewModels
{
    public class RegisterViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        // Properties
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
                ClearErrors(nameof(Email));
                
                if (string.IsNullOrWhiteSpace(Email))
                {
                    AddError("Email mag niet leeg zijn.", nameof(Email));
                }
                
                OnPropertyChanged(nameof(CanCreateAccount));
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

        // Commands
        public ICommand GoToLoginButton { get; }
        public ICommand RegisterButton { get; }
        
        // Constructor
        public RegisterViewModel(NavigationService loginNavigationService, DatabaseConnection connection) 
        {
            _connection = connection;
            GoToLoginButton = new NavigateCommand(loginNavigationService);
            RegisterButton = new RegisterStudentCommand(this, connection);

            _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
        }

        // Data validation
        public bool CanCreateAccount => !HasErrors;

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;
        public bool HasErrors => _propertyNameToErrorsDictionary.Any();
        
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        
        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
        }

        private void ClearErrors(string propertyName)
        {
            _propertyNameToErrorsDictionary.Remove(propertyName);
            OnErrorsChanged(propertyName);
            ErrorMessage = null;
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        
        private void AddError(string errorMessage, string propertyName)
        {
            if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                _propertyNameToErrorsDictionary.Add(propertyName, new List<string>());
            }
            
            _propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
            ErrorMessage = errorMessage;
        }
        
    }
}
