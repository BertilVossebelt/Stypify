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

        public LoginCommand(LoginViewModel loginViewModel,
            NavigationService studentDashboardNavigationService, NavigationService adminDashboardNavigationService,
            NavigationService teacherDashboardNavigationService, UserStore userStore)
        {
            _loginViewModel = loginViewModel;
            _studentDashboardNavigationService = studentDashboardNavigationService;
            _adminDashboardNavigationService = adminDashboardNavigationService;
            _teacherDashboardNavigationService = teacherDashboardNavigationService;
            _userStore = userStore;
        }

        public override void Execute(object? parameter)
        {
            string password = SecureStringToString(_loginViewModel.Password);
            var isValidUser = AuthenticateUser(new NetworkCredential(_loginViewModel.Email, password));
            if (!isValidUser)
            {
                ShowIncorrectCredentialsMessage();
                return;
            }
            
            var navigateCommand = new NavigateCommand(_studentDashboardNavigationService);
            var user = new UserProvider().GetById(_userId);
            if (user == null) return;

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
            }
            
            navigateCommand.Execute(this);
        }

        private bool AuthenticateUser(NetworkCredential credential)
        {
            var userProvider = new UserProvider();
            var validUser = userProvider.VerifyUser(credential.UserName, credential.Password);

            if (!validUser) return validUser;
            
            _userId = userProvider.WeirdThingAgainId(credential.UserName);
            return validUser;
        }

        private bool IsTeacherAccount()
        {
            var userProvider = new UserProvider();
            return userProvider.WeirdThingTeacher(_loginViewModel.Email);
        }

        private bool IsAdminAccount()
        {
            var userProvider = new UserProvider();
            return userProvider.WeirdThingAdmin(_loginViewModel.Email);
        }

        private static void ShowIncorrectCredentialsMessage()
        {
            const string message = "Email of wachtwoord incorrect.";
            const MessageBoxButton type = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(message, "Error", type, icon);
        }
        
        // Convert de SecureString naar een string. (kan even niet anders)
        private static string? SecureStringToString(SecureString value)
        {
            var valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}