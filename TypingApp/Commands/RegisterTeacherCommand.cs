using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using TypingApp.Models;
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

    public override bool CanExecute(object? parameter)
    {
        return _adminDashboardViewModel.CanCreateAccount;
    }

    /*
     * Register a new teacher account.
     * --------------------------------------
     * Method should only be used for admins.
     */
    public override void Execute(object? parameter)
    {
        string? message;
        var teacher = new TeacherProvider().GetByEmail(_adminDashboardViewModel.Email);
        if (teacher != null)
        {
            message = "Er bestaat al een docent met dit e-mailadres.";
            MessageBox.Show(message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            // Try to create a new teacher account.
            var password = SecureStringToString(_adminDashboardViewModel.Password);
            var hash = new PasswordHash(password);
            var hashedPassword = hash.ToArray();
            var salt = hash.Salt;

            // Add the new teacher to the database using the teacher provider.
            new TeacherProvider().Create(_adminDashboardViewModel.Email, hashedPassword, salt,
                _adminDashboardViewModel.FirstName, _adminDashboardViewModel.Preposition,
                _adminDashboardViewModel.LastName);

            // Notify the user that the account has been created.
            message = "Account succesvol aangemaakt.";
            MessageBox.Show(message, "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception e)
        {
            // Display an error message if the account could not be created.
            MessageBox.Show(e.ToString());
        }
    }
    
    /*
     * Convert a SecureString to a string.
     */
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