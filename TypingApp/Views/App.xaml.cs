using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;
using TypingApp.ViewModels;
using TypingApp.Commands;

namespace TypingApp.Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly User _user;
        private readonly ExerciseStore _exerciseStore;
        private readonly NavigationStore _navigationStore;
        private readonly DatabaseConnection _connection;

        public App()
        {
            var characters = new List<Character>()
            {
                new('e'),
                new('n'),
                new('a'),
                new('t'),
            };

            _user = new User(11, "email@email.nl", "Voornaam", "Achternaam", characters, true);
            _navigationStore = new NavigationStore();
            _connection = new DatabaseConnection();
            _exerciseStore = new ExerciseStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // _navigationStore.CurrentViewModel = new GroupViewModel(_connection);
            
            _navigationStore.CurrentViewModel = CreateLoginViewModel();

            MainWindow = new MainWindow(_navigationStore, _exerciseStore, _user)
            {
                DataContext = new MainViewModel(_navigationStore, _connection)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        private LoginViewModel CreateLoginViewModel()
        {
            var registerViewModel = new NavigationService(_navigationStore, CreateRegisterViewModel);
            var adminDashboardViewModel = new NavigationService(_navigationStore, CreateAdminDashboardViewModel);
            var studentDashboardViewModel = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            
            return new LoginViewModel(registerViewModel, adminDashboardViewModel, studentDashboardViewModel, teacherDashboardViewModel, _connection);
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
            return new StudentDashboardViewModel(exerciseNavigationService, linkToGroupNavigationService);
        }
        private ExerciseViewModel CreateExerciseViewModel()
        {
            return new ExerciseViewModel(new NavigationService(_navigationStore, CreateStudentDashboardViewModel), _user, _exerciseStore);
        }

        private TeacherDashboardViewModel CreateTeacherDashboardViewModel()
        {
            return new TeacherDashboardViewModel(new NavigationService(_navigationStore, CreateAddGroupViewModel), _user, _connection);
        }

        private AddGroupViewModel CreateAddGroupViewModel()
        {
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            var studentDashboardViewModel = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);
            return new AddGroupViewModel(studentDashboardViewModel, teacherDashboardViewModel, _user, _connection);
        }

        private LinkToGroupViewModel CreateLinkToGroupViewModel()
        {
            var teacherDashboardViewModel = new NavigationService(_navigationStore, CreateTeacherDashboardViewModel);
            var studentDashboardViewModel = new NavigationService(_navigationStore, CreateStudentDashboardViewModel);

            return new LinkToGroupViewModel(studentDashboardViewModel, teacherDashboardViewModel, _user, _connection);
        }
    }
}