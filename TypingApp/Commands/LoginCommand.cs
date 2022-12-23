using System;
using System.Collections.Generic;
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
        private readonly LessonStore _lessonStore;
        private readonly NavigationService _studentDashboardNavigationService;
        private readonly NavigationService _adminDashboardNavigationService;
        private readonly NavigationService _teacherDashboardNavigationService;

        private int _userId { get; set; }

        public LoginCommand(LoginViewModel loginViewModel,
            NavigationService studentDashboardNavigationService, NavigationService adminDashboardNavigationService,
            NavigationService teacherDashboardNavigationService, UserStore userStore, LessonStore lessonStore)
        {
            _loginViewModel = loginViewModel;
            _studentDashboardNavigationService = studentDashboardNavigationService;
            _adminDashboardNavigationService = adminDashboardNavigationService;
            _teacherDashboardNavigationService = teacherDashboardNavigationService;
            _userStore = userStore;
            _lessonStore = lessonStore;
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
            
            // Change login routine based on the role of the user.
            if ((byte)user["teacher"] == 1)
            {
                _userStore.CreateTeacher(user);
                navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
            }
            else if ((byte)user["admin"] == 1)
            {
                _userStore.CreateAdmin(user);
                navigateCommand = new NavigateCommand(_adminDashboardNavigationService);
            }
            else
            {
                _userStore.CreateStudent(user);
                _lessonStore.LoadLessons(_userStore); // Fetch all available lessons for the student.

            }

            navigateCommand.Execute(this);
        }

        // Check if given credentials match.
        private bool AuthenticateUser(NetworkCredential credential)
        {
            var userProvider = new UserProvider();
            var user = userProvider.GetByCredentials(credential.UserName);

            // Check if user exists.
            if (user == null) return false;
            var hashedPassword = (byte[])user["password"];
            var salt = (byte[])user["salt"];
            var correct = new PasswordHash(hashedPassword).Verify(credential.Password, salt);

            // Exit if password is incorrect.
            if (!correct) return false;
            _userId = (int)user["id"];

            return true;
        }

        // Display error message when given credentials are incorrect.
        private static void ShowIncorrectCredentialsMessage()
        {
            const string message = "Email of wachtwoord incorrect.";
            const MessageBoxButton type = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(message, "Error", type, icon);
        }
    }
}