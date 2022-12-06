using System.Security;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.Database;

namespace TypingApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email;
        private SecureString _password;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoToRegisterButton { get; }
        public ICommand LoginButton { get; }

        public LoginViewModel(NavigationService registerNavigationService,
            NavigationService adminDashboardNavigationService, NavigationService studentDashboardNavigationService,
            NavigationService teacherDashboardNavigationService,
            DatabaseService connection,
            User user)
        {
            
            _connection = connection;
            GoToRegisterButton = new NavigateCommand(registerNavigationService);
            LoginButton = new LoginCommand(this, _connection, studentDashboardNavigationService,
                adminDashboardNavigationService, teacherDashboardNavigationService,user);
        }
    }
}