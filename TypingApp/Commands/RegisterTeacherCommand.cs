using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using TypingApp.Services;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class RegisterTeacherCommand : CommandBase
{
    private readonly DatabaseService _connection;
    private readonly AdminDashboardViewModel _adminDashboardViewModel;

    public RegisterTeacherCommand(AdminDashboardViewModel adminDashboardViewModel, DatabaseService connection)
    {
        _adminDashboardViewModel = adminDashboardViewModel;
        _connection = connection;
    }

    public override void Execute(object? parameter)
    {
        // TODO: Refactoren, te lange en ingewikkelde functie. Gebruik Providers!
        var password = SecureStringToString(_adminDashboardViewModel.Password);
        var passwordConfirm = SecureStringToString(_adminDashboardViewModel.PasswordConfirm);

        if (password != null && !password.Equals(passwordConfirm))
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
                var command = new SqlCommand();
                command.Connection = _connection.GetConnection();

                command.CommandText =
                    "INSERT INTO [Users] (teacher, student, email, password, first_name, preposition, last_name, admin)" +
                    "VALUES (@teacher, @student, @email, @password, @first_name, @preposition, @last_name, @admin)";

                command.Parameters.Add("@teacher", SqlDbType.TinyInt).Value = 1;
                command.Parameters.Add("@student", SqlDbType.TinyInt).Value = 0;
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _adminDashboardViewModel.Email;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;

                if (!string.IsNullOrEmpty(_adminDashboardViewModel.Preposition))
                {
                    command.Parameters.Add("@preposition", SqlDbType.NVarChar).Value =
                        _adminDashboardViewModel.Preposition;
                }
                else
                {
                    command.Parameters.AddWithValue("@preposition", DBNull.Value);
                }

                command.Parameters.Add("@first_name", SqlDbType.NVarChar).Value = _adminDashboardViewModel.FirstName;
                command.Parameters.Add("@last_name", SqlDbType.NVarChar).Value = _adminDashboardViewModel.LastName;
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

    private bool DoesAccountExist()
    {
        var command = new SqlCommand();
        command.Connection = _connection.GetConnection();
        command.CommandText = "SELECT * FROM [Users] WHERE email=@email";
        command.Parameters.Add("@email", SqlDbType.NVarChar).Value = _adminDashboardViewModel.Email;
        return command.ExecuteScalar() != null;
    }
    
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