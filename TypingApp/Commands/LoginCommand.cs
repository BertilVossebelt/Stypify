using System;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.ViewModels;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly UserStore _userStore;
        private readonly NavigationService _studentDashboardNavigationService;
        private readonly NavigationService _adminDashboardNavigationService;
        private readonly NavigationService _teacherDashboardNavigationService;

        private int _userId { get; set; }
        public NavigationService bla { get; }

        public LoginCommand(LoginViewModel loginViewModel,
            NavigationService studentDashboardNavigationService, NavigationService adminDashboardNavigationService,
            NavigationService teacherDashboardNavigationService, UserStore userStore, NavigationService createLessonViewModel)
        {
            bla = createLessonViewModel;
            _loginViewModel = loginViewModel;
            _studentDashboardNavigationService = studentDashboardNavigationService;
            _adminDashboardNavigationService = adminDashboardNavigationService;
            _teacherDashboardNavigationService = teacherDashboardNavigationService;
            _userStore = userStore;
        }

        public override void Execute(object? parameter)
        {
            var isValidUser = AuthenticateUser(new NetworkCredential(_loginViewModel.Email, _loginViewModel.Password));
            if (!isValidUser)
            {
                ShowIncorrectCredentialsMessage();
                return;
            }
            
            var navigateCommand = new NavigateCommand(_studentDashboardNavigationService);
            var user = new UserProvider().GetById(_userId);
            if (user == null) return;

            // Check of de user een docent, admin of student is.
            if ((byte)user["teacher"] == 1)
            {
                _userStore.CreateTeacher(user);
                navigateCommand = new NavigateCommand(bla);
            }
            else if ((byte)user["admin"] == 1)
            {
                _userStore.CreateAdmin(user);
                navigateCommand = new NavigateCommand(_adminDashboardNavigationService);
            }
            else
            {
                _userStore.CreateStudent(user);
            }
            
            navigateCommand.Execute(this);
        }

        // Check of de ingevulde account gegevens kloppen.
        private bool AuthenticateUser(NetworkCredential credential)
        {
            var userProvider = new UserProvider();
            var validUser = userProvider.VerifyUser(credential.UserName, credential.Password);

            if (!validUser) return validUser;
            
            _userId = userProvider.GetUserId(credential.UserName);
            return validUser;
        }

        // Laat een error message zien als de accountgegevens niet kloppen.
        private static void ShowIncorrectCredentialsMessage()
        {
            const string message = "Email of wachtwoord incorrect.";
            const MessageBoxButton type = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(message, "Error", type, icon);
        }
    }
}