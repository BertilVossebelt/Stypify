using System.Security;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;

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
            NavigationService teacherDashboardNavigationService, UserStore userStore, NavigationService createLessonViewModel )
        {
            GoToRegisterButton = new NavigateCommand(registerNavigationService);
            LoginButton = new LoginCommand(this, studentDashboardNavigationService,
                adminDashboardNavigationService, teacherDashboardNavigationService, userStore, createLessonViewModel);
        }
    }
}