using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;

namespace TypingApp.Commands
{
    public class RegisterStudentCommand : CommandBase
    {
        private readonly DatabaseConnection _connection;
        private readonly RegisterViewModel _registerViewModel;

        public RegisterStudentCommand(RegisterViewModel registerViewModel, DatabaseConnection connection)
        {
            _registerViewModel = registerViewModel;
            _connection = connection;
        }

        public override bool CanExecute(object? parameter)
        {
            if (_registerViewModel.CanCreateAccount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Execute(object? parameter)
        {
            if (DoesAccountExist())
            {
               ShowAccountAlreadyExistsError();
            }
            else
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = _connection.GetConnection();

                    command.CommandText =
                        "INSERT INTO [Users] (teacher, student, email, password, first_name, preposition, last_name, admin)" +
                        "VALUES (@teacher, @student, @email, @password, @first_name, @preposition, @last_name, @admin)";

                    command.Parameters.Add("@teacher", SqlDbType.TinyInt).Value = 0;
                    command.Parameters.Add("@student", SqlDbType.TinyInt).Value = 1;
                    command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _registerViewModel.Email;
                    command.Parameters.Add("@password", SqlDbType.NVarChar).Value = _registerViewModel.Password;

                    if (!string.IsNullOrWhiteSpace(_registerViewModel.Preposition))
                    {
                        command.Parameters.Add("@preposition", SqlDbType.NVarChar).Value = _registerViewModel.Preposition;
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@preposition", DBNull.Value);
                    }

                    command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = _registerViewModel.FirstName;
                    command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = _registerViewModel.LastName;
                    command.Parameters.Add("@admin", SqlDbType.TinyInt).Value = 0;

                    command.ExecuteNonQuery();
                    MessageBox.Show("Account succesvol aangemaakt", "Account aangemaakt", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
            }
        }

        // Check of het account al bestaat.
        private bool DoesAccountExist()
        {
            bool doesAccountExist;
            SqlCommand command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _registerViewModel.Email;
            doesAccountExist = command.ExecuteScalar() == null ? false : true;

            return doesAccountExist;
        }
        
        // Laat een error message zien als het account al bestaat.
        private void ShowAccountAlreadyExistsError()
        {
            const string message = "Er bestaat al een account met dit emailadres.";
            const MessageBoxButton type = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;
            
            MessageBox.Show(message, "Bestaand account", type, icon);
        }
        
        private string HashPassword(SecureString password)
        {
            return "test";
        }
    }
}
