using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.ViewModels;

namespace TypingApp.Commands
{
    public class RegisterStudentCommand : CommandBase
    {
        private readonly RegisterViewModel _registerViewModel;

        public RegisterStudentCommand(RegisterViewModel registerViewModel)
        {
            _registerViewModel = registerViewModel;
        }

        // Check of het registercommando uitgevoerd mag worden.
        public override bool CanExecute(object? parameter)
        {
            return _registerViewModel.CanCreateAccount;
        }

        // Voer het registercommando uit.
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
                    string password = SecureStringToString(_registerViewModel.Password);
                    PasswordHash hash = new PasswordHash(password);
                    byte[] hashedPassword = hash.ToArray();
                    byte[] salt = hash.Salt;
                    
                    new StudentProvider().Create(_registerViewModel.Email, hashedPassword, salt,
                        _registerViewModel.Preposition, _registerViewModel.FirstName, _registerViewModel.LastName);

                    ShowAccountSuccesfullyCreatedMessage();
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
            var student = new StudentProvider().GetByEmail(_registerViewModel.Email);
            return student != null;
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
}