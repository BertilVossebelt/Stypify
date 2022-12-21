using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class RegisterTeacherCommand : CommandBase
{
    private readonly AdminDashboardViewModel _adminDashboardViewModel;

    public RegisterTeacherCommand(AdminDashboardViewModel adminDashboardViewModel)
    {
        _adminDashboardViewModel = adminDashboardViewModel;
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
                new AdminProvider().RegisterTeacher(_adminDashboardViewModel.Email, _adminDashboardViewModel.Password,
                    _adminDashboardViewModel.Preposition, _adminDashboardViewModel.FirstName,
                    _adminDashboardViewModel.LastName);
                
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
        var teacher = new TeacherProvider().GetByEmail(_adminDashboardViewModel.Email);
        return teacher != null;
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