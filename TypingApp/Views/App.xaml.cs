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
        private readonly NavigationStore _navigationStore;
        
        public App()
        {
            var characters = new List<Character>()
            {
                new(1, 'e', 0),
                new(1, 'n', 0),
                new(1, 'a', 0),
                new(1, 't', 0),
            };
            
            _user = new User(1, "email@email.nl", "Voornaam", "Achternaam", characters,true);
            _navigationStore = new NavigationStore();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            // dotnet ef migrations add InitialMigration --project TypingApp
            if (_user.IsTeacher)
            {
                _navigationStore.CurrentViewModel = new TeacherDashboardViewModel(_user, _navigationStore);
            }
            else
            {
                _navigationStore.CurrentViewModel = new StudentDashboardViewModel(_user, _navigationStore);
            }

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            
            MainWindow.Show();
            
            base.OnStartup(e);
        }
    }
}