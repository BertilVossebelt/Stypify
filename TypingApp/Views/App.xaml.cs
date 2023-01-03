using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
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
        private readonly LessonStore _lessonStore;

        public App()
        {
            // Setup stores.
            _navigationStore = new NavigationStore();
            _exerciseStore = new ExerciseStore();
            _userStore = new UserStore();
            _exerciseStore = new ExerciseStore();
            _lessonStore = new LessonStore(_userStore); // Needs to be initialized after user store.
        }
        
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Error" + Environment.NewLine + e.Exception.Message, "Error");
            e.Handled = true;
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

        private LoginViewModel CreateLoginViewModel()
        {
            var registerNavigationService = new NavigationService(_navigationStore, CreateRegisterViewModel);
            var adminDashboardNavigationService = new NavigationService(_navigationStore, CreateAdminDashboardViewModel);
            var studentDashboardNavigationService = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);
            var teacherDashboardNavigationService = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);

            return new LoginViewModel(registerNavigationService, adminDashboardNavigationService, studentDashboardNavigationService,
                teacherDashboardNavigationService, _userStore, _lessonStore);
        }

        private AdminDashboardViewModel CreateAdminDashboardViewModel()
        {
            var loginNavigationService = new NavigationService(_navigationStore, CreateLoginViewModel);
            return new AdminDashboardViewModel(_userStore, loginNavigationService);
        }

        private RegisterViewModel CreateRegisterViewModel()
        {
            var loginNavigationService = new NavigationService(_navigationStore, CreateRegisterViewModel);

            return new RegisterViewModel(loginNavigationService);
        }

        private StudentDashboardViewModel CreateStudentDashboardViewModel()
        {
            var exerciseNavigationService = new NavigationService(_navigationStore, CreateExerciseViewModel);
            var linkToGroupNavigationService = new NavigationService(_navigationStore, CreateLinkToGroupViewModel);
            var loginNavigationService = new NavigationService(_navigationStore, CreateLoginViewModel);
            var lessonNavigationService = new NavigationService(_navigationStore, CreateLessonViewModel);

            return new StudentDashboardViewModel(_userStore, _lessonStore, exerciseNavigationService, linkToGroupNavigationService,
                loginNavigationService, lessonNavigationService);
        }

        private LessonViewModel CreateLessonViewModel()
        {
            var studentDashboardNavigationService = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);
            
            return new LessonViewModel(studentDashboardNavigationService, _lessonStore, _userStore);
        }

        private ExerciseViewModel CreateExerciseViewModel()
        {
            return new ExerciseViewModel(new NavigationService(_navigationStore, CreateStudentDashboardViewModel),
                _userStore, _exerciseStore);
        }

        private TeacherDashboardViewModel CreateTeacherDashboardViewModel()
        {
            var myLessonsViewModel = new NavigationService(_navigationStore, CreateMyLessonsViewModel);
            var createAddGroupViewModel = new NavigationService(_navigationStore, CreateAddGroupViewModel);
            var loginNavigationService = new NavigationService(_navigationStore, CreateLoginViewModel);

            return new TeacherDashboardViewModel(createAddGroupViewModel, myLessonsViewModel, loginNavigationService, _userStore);
        }

        private MyLessonsViewModel CreateMyLessonsViewModel()
        {
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            var createExerciseViewModel = new NavigationService(_navigationStore, CreateCreateExerciseViewModel);
            var createLessonExerciseViewModel = new NavigationService(_navigationStore, CreateCreateLessonViewModel);
            
            return new MyLessonsViewModel(teacherDashboardViewModel, createExerciseViewModel, createLessonExerciseViewModel, _userStore,_lessonStore);
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

        private CreateExerciseViewModel CreateCreateExerciseViewModel()
        {
            var myLessonsNavigationService = new NavigationService(_navigationStore, CreateMyLessonsViewModel);

            return new CreateExerciseViewModel(myLessonsNavigationService, _userStore);
        }

        //Viewmodel does not exist yet so this a temp viewmodel because it will throw notImplemented
        private TeacherDashboardViewModel CreateCreateLessonViewModel()
        {
            var myLessonNavigationService = new NavigationService(_navigationStore, CreateMyLessonsViewModel);

            throw new NotImplementedException();
            //Gives lessonStore to AddLessonViewModel (Which adds and edits lessons) so AddLessonViewModel knows which lessons it is editing,
            //Should return somethinng like this:
            //return new CreateLessonViewModel(myLessonNavigationService, _userStore, _lessonStore);
        }
    }
}