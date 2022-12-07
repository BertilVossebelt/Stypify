using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly User _user;
        private readonly ExerciseStore _exerciseStore;
        private readonly UserStore _userStore;
        private readonly NavigationStore _navigationStore;
        private readonly DatabaseService _connection;

        public App()
        {
            _navigationStore = new NavigationStore();
            // _connection = new DatabaseService();
            _exerciseStore = new ExerciseStore();
            _userStore = new UserStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = CreateLoginViewModel();

            MainWindow = new MainWindow(_exerciseStore, _userStore)
            {
                DataContext = new MainViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        private LoginViewModel? CreateLoginViewModel()
        {
            var registerViewModel = new NavigationService(_navigationStore, CreateRegisterViewModel);
            var adminDashboardViewModel = new NavigationService(_navigationStore, CreateAdminDashboardViewModel);
            var studentDashboardViewModel = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            
            return new LoginViewModel(registerViewModel, adminDashboardViewModel, studentDashboardViewModel, teacherDashboardViewModel, _connection, _userStore);
        }

        private AdminDashboardViewModel CreateAdminDashboardViewModel()
        {
            return new AdminDashboardViewModel(_connection);
        }

        private RegisterViewModel CreateRegisterViewModel()
        {
            return new RegisterViewModel(new NavigationService(_navigationStore, CreateLoginViewModel), _connection);
        }

        private StudentDashboardViewModel CreateStudentDashboardViewModel()
        {
            var exerciseNavigationService = new NavigationService(_navigationStore, CreateExerciseViewModel);
            var linkToGroupNavigationService = new NavigationService(_navigationStore, CreateLinkToGroupViewModel);
            var loginNavigationService = new NavigationService(_navigationStore, CreateLoginViewModel);
            return new StudentDashboardViewModel(_userStore, _connection ,exerciseNavigationService, linkToGroupNavigationService, loginNavigationService);
        }
        private ExerciseViewModel CreateExerciseViewModel()
        {
            return new ExerciseViewModel(new NavigationService(_navigationStore, CreateStudentDashboardViewModel), _userStore, _exerciseStore);
        }

        private GroupViewModel CreateTeacherDashboardViewModel()
        {
            return new GroupViewModel(new NavigationService(_navigationStore, CreateAddGroupViewModel), _userStore, _connection);
        }

        private AddGroupViewModel CreateAddGroupViewModel()
        {
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            var studentDashboardViewModel = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);
            return new AddGroupViewModel(studentDashboardViewModel, teacherDashboardViewModel, _userStore, _connection);
        }

        private LinkToGroupViewModel CreateLinkToGroupViewModel()
        {
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            var studentDashboardViewModel = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);

            return new LinkToGroupViewModel(studentDashboardViewModel, teacherDashboardViewModel, _userStore, _connection);
        }
    }
}