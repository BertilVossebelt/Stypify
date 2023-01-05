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
        public ICommand GoToRegisterButton { get; }
        public ICommand LoginButton { get; }
        public string Email { get; set; }
        public SecureString Password { get; set; }

        public LoginViewModel(NavigationService registerNavigationService,
            NavigationService adminDashboardNavigationService, NavigationService studentDashboardNavigationService,
            NavigationService teacherDashboardNavigationService, UserStore userStore, LessonStore lessonStore)
        {
            
            GoToRegisterButton = new NavigateCommand(registerNavigationService);

            LoginButton = new LoginCommand(this, studentDashboardNavigationService,
                adminDashboardNavigationService, teacherDashboardNavigationService, userStore, lessonStore);
        }
    }
}