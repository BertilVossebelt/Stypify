using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using TypingApp.Services;
using TypingApp.Services.Database;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    public class RegisterStudentCommand : CommandBase
    {
        private readonly DatabaseService _connection;
        private readonly RegisterViewModel _registerViewModel;

        public RegisterStudentCommand(RegisterViewModel registerViewModel, DatabaseService connection)
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
            string password = SecureStringToString(_registerViewModel.Password);
            string passwordConfirm = SecureStringToString(_registerViewModel.PasswordConfirm);

            if (!PasswordConfirmCorrect(password, passwordConfirm))
            {
                MessageBox.Show("De twee wachtwoorden moeten gelijk zijn.", "Wachtwoorden niet gelijk",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            else if (DoesAccountExist())
            {
                
                MessageBox.Show("Er bestaat al een account met dit emailadres.", "Bestaand account",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
                    command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;

                    if (!string.IsNullOrEmpty(_registerViewModel.Preposition))
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

        public bool DoesAccountExist()
        {
            bool doesAccountExist;
            SqlCommand command = new SqlCommand();
            command.Connection = _connection.GetConnection();

            command.CommandText = "SELECT * FROM [Users] WHERE email=@email";
            command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _registerViewModel.Email;
            doesAccountExist = command.ExecuteScalar() == null ? false : true;

            return doesAccountExist;
        }

        public bool PasswordConfirmCorrect(string password, string passwordConfirm)
        {
            if (password.Equals(passwordConfirm))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
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
