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

    // Voer het registercommando uit.
    public override void Execute(object? parameter)
    {
        if (DoesAccountExist())
            ShowAccountAlreadyExistsError();
        else
            try
            {
                var password = SecureStringToString(_adminDashboardViewModel.Password);
                PasswordHash hash = new PasswordHash(password);
                byte[] hashedPassword = hash.ToArray();
                byte[] salt = hash.Salt;

                new TeacherProvider().Create(_adminDashboardViewModel.Email, hashedPassword, salt,
                    _adminDashboardViewModel.FirstName, _adminDashboardViewModel.Preposition, _adminDashboardViewModel.LastName);

                ShowAccountSuccesfullyCreatedMessage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
    }

    // Check of het account al bestaat.
    private bool DoesAccountExist()
    {
        var teacher = new TeacherProvider().GetByEmail(_adminDashboardViewModel.Email);
        return teacher != null;
    }

    // Laat een error message zien als het account al bestaat.
    private void ShowAccountAlreadyExistsError()
    {
        const string message = "Er bestaat al een account met dit emailadres.";
        const MessageBoxButton type = MessageBoxButton.OK;
        const MessageBoxImage icon = MessageBoxImage.Error;

        MessageBox.Show(message, "Bestaand account", type, icon);
    }

    // Laat een error message zien als het account al bestaat.
    private void ShowAccountSuccesfullyCreatedMessage()
    {
        const string message = "Account succesvol aangemaakt.";
        const MessageBoxButton type = MessageBoxButton.OK;
        const MessageBoxImage icon = MessageBoxImage.Information;

        MessageBox.Show(message, "Account aangemaakt", type, icon);
    }

    // Convert de SecureString naar een string. (kan niet anders op dit moment)
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