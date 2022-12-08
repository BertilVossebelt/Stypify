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
        private readonly NavigationStore _navigationStore;
        private readonly ExerciseStore _exerciseStore;
        private readonly UserStore _userStore;

        public App()
        {
            _navigationStore = new NavigationStore();
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
            
            return new LoginViewModel(registerViewModel, adminDashboardViewModel, studentDashboardViewModel, teacherDashboardViewModel, _userStore);
        }

        private AdminDashboardViewModel CreateAdminDashboardViewModel()
        {
            return new AdminDashboardViewModel();
        }

        private RegisterViewModel CreateRegisterViewModel()
        {
            return new RegisterViewModel(new NavigationService(_navigationStore, CreateLoginViewModel));
        }

        private StudentDashboardViewModel CreateStudentDashboardViewModel()
        {
            var exerciseNavigationService = new NavigationService(_navigationStore, CreateExerciseViewModel);
            var linkToGroupNavigationService = new NavigationService(_navigationStore, CreateLinkToGroupViewModel);
            var loginNavigationService = new NavigationService(_navigationStore, CreateLoginViewModel);
            return new StudentDashboardViewModel(_userStore, exerciseNavigationService, linkToGroupNavigationService, loginNavigationService);
        }
        private ExerciseViewModel CreateExerciseViewModel()
        {
            return new ExerciseViewModel(new NavigationService(_navigationStore, CreateStudentDashboardViewModel), _userStore, _exerciseStore);
        }

        private TeacherDashboardViewModel CreateTeacherDashboardViewModel()
        {
            return new TeacherDashboardViewModel(new NavigationService(_navigationStore, CreateAddGroupViewModel), _userStore);
        }

        private AddGroupViewModel CreateAddGroupViewModel()
        {
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            var studentDashboardViewModel = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);
            return new AddGroupViewModel(studentDashboardViewModel, teacherDashboardViewModel, _userStore);
        }

        private LinkToGroupViewModel CreateLinkToGroupViewModel()
        {
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            var studentDashboardViewModel = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);

            return new LinkToGroupViewModel(studentDashboardViewModel, teacherDashboardViewModel, _userStore);
        }
    }
}