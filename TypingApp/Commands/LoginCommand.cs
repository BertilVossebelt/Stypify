using System;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Windows;
using TypingApp.Models;
using TypingApp.ViewModels;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly DatabaseConnection _connection;
        private readonly LoginViewModel _loginViewModel;
        private readonly User _user;
        private readonly NavigationService _studentDashboardNavigationService;
        private readonly NavigationService _adminDashboardNavigationService;
        private readonly NavigationService _teacherDashboardNavigationService;

        public LoginCommand(LoginViewModel loginViewModel, DatabaseConnection connection,
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
            if (!isValidUser) ShowIncorrectCredentialsMessage();

            if (isStudentAccount())
            {
                var navigateCommand = new NavigateCommand(_studentDashboardNavigationService);
                navigateCommand.Execute(this);
            }

            if (isTeacherAccount())
            {
                var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
                navigateCommand.Execute(this);
            }

            if (isAdminAccount())
            {
                var navigateCommand = new NavigateCommand(_adminDashboardNavigationService);
                navigateCommand.Execute(this);
            }
        }

        private static void ShowIncorrectCredentialsMessage()
        {
            const string message = "Email of wachtwoord incorrect.";
            const MessageBoxButton type = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;
            MessageBox.Show(message, "Error", type, icon);
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            SqlCommand command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email and [password]=@password";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = credential.UserName;
            command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
            validUser = command.ExecuteScalar() == null ? false : true;
            if (validUser)
            {
                int userId = (int)command.ExecuteScalar();
                Console.WriteLine(userId);
                _user.Id = userId;
            }

            return validUser;
        }

        public bool isStudentAccount()
        {
            bool isStudentAccount;
            SqlCommand command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND student = 1";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _loginViewModel.Email;
            isStudentAccount = command.ExecuteScalar() == null ? false : true;
            _user.IsTeacher = false;
            return isStudentAccount;
        }

        public bool isTeacherAccount()
        {
            bool isTeacherAccount;
            SqlCommand command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND teacher = 1";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _loginViewModel.Email;
            isTeacherAccount = command.ExecuteScalar() == null ? false : true;
            _user.IsTeacher = true;
            return isTeacherAccount;
        }

        public bool isAdminAccount()
        {
            bool isAdminAccount;
            SqlCommand command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email AND admin = 1";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _loginViewModel.Email;
            isAdminAccount = command.ExecuteScalar() == null ? false : true;

            return isAdminAccount;
        }
    }
}