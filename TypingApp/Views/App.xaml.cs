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
        private readonly NavigationStore _navigationStore;

        public App()
        {
            var characters = new List<Character>()
            {
                new('e'),
                new('n'),
                new('a'),
                new('t'),
            };

            _user = new User(1, "email@email.nl", "Voornaam", "Achternaam", characters);
            _navigationStore = new NavigationStore();
            _exerciseStore = new ExerciseStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = CreateStudentDashboardViewModel();

            MainWindow = new MainWindow(_navigationStore, _exerciseStore, _user)
            {
                DataContext = new MainViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        private StudentDashboardViewModel CreateStudentDashboardViewModel()
        {
            return new StudentDashboardViewModel(new NavigationService(_navigationStore, CreateExerciseViewModel));
        }
        private ExerciseViewModel CreateExerciseViewModel()
        {
            return new ExerciseViewModel(new NavigationService(_navigationStore, CreateStudentDashboardViewModel), _user, _exerciseStore);
        }
    }
}