using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TypingApp.Models;
using TypingApp.Stores;
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
        private NavigationService _teacherDashboardNavigationService;

        public LoginCommand(LoginViewModel loginViewModel, DatabaseConnection connection, NavigationService studentDashboardNavigationService, NavigationService adminDashboardNavigationService, NavigationService teacherDashboardNavigationService)
        {
            _loginViewModel = loginViewModel;
            _connection = connection;
            _studentDashboardNavigationService = studentDashboardNavigationService;
            _adminDashboardNavigationService = adminDashboardNavigationService;
            _teacherDashboardNavigationService = teacherDashboardNavigationService;
        }

        public override void Execute(object? parameter)
        {
            var isValidUser = AuthenticateUser(new NetworkCredential(_loginViewModel.Email, _loginViewModel.Password));
            if (isValidUser)
            {
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
            else
            {
                MessageBox.Show("Email of wachtwoord incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
