using System;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.Database;
using TypingApp.ViewModels;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly DatabaseService _connection;
        private readonly LoginViewModel _loginViewModel;
        private readonly User _user;
        private readonly NavigationService _studentDashboardNavigationService;
        private readonly NavigationService _adminDashboardNavigationService;
        private readonly NavigationService _teacherDashboardNavigationService;

        public LoginCommand(LoginViewModel loginViewModel, DatabaseService connection,
            NavigationService studentDashboardNavigationService, NavigationService adminDashboardNavigationService,
            NavigationService teacherDashboardNavigationService, User user)
        {
            _loginViewModel = loginViewModel;
            _connection = connection;
            _studentDashboardNavigationService = studentDashboardNavigationService;
            _adminDashboardNavigationService = adminDashboardNavigationService;
            _teacherDashboardNavigationService = teacherDashboardNavigationService;
            _user = user;
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
            if (IsAdminAccount()) navigateCommand = new NavigateCommand(_adminDashboardNavigationService);
            if (IsTeacherAccount()) navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
            navigateCommand.Execute(this);
        }

        private bool AuthenticateUser(NetworkCredential credential)
        {
            var command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email and [password]=@password";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = credential.UserName;
            command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
            var validUser = command.ExecuteScalar() != null;
            
            if (!validUser) return validUser;
            
            var userId = (int)command.ExecuteScalar();
            _user.Id = userId;
            return validUser;
        }
        
        private bool IsTeacherAccount()
        {
            var command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND teacher = 1";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _loginViewModel.Email;
            var isTeacherAccount = command.ExecuteScalar() != null;
            _user.IsTeacher = true;
            return isTeacherAccount;
        }

        private bool IsAdminAccount()
        {
            var command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND admin = 1";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _loginViewModel.Email;
            var isAdminAccount = command.ExecuteScalar() != null;

            return isAdminAccount;
        }
        
        private static void ShowIncorrectCredentialsMessage()
        {
            const string message = "Email of wachtwoord incorrect.";
            const MessageBoxButton type = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(message, "Error", type, icon);
        }
    }
}