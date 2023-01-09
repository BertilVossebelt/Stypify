using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Services.PasswordHash;
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

        // Check if register command is allowed to execute.
        public override bool CanExecute(object? parameter)
        {
            return _registerViewModel.CanCreateAccount;
        }
        
        /*
         * Register a new student account.
         * -------------------------------------
         * Only used when user is not logged in.
         */
        public override void Execute(object? parameter)
        {
            string? message;
            
            var student = new StudentProvider().GetByEmail(_registerViewModel.Email);
            if (student != null)
            { 
                message = "Er bestaat al een leerling met dit e-mailadres.";
                MessageBox.Show(message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Try to register the student.
                var password = SecureStringToString(_registerViewModel.Password);
                var hash = new PasswordHash(password);
                var hashedPassword = hash.ToArray();
                var salt = hash.Salt;

                // Add the student to the database.
                new StudentProvider().Create(_registerViewModel.Email, hashedPassword, salt,
                    _registerViewModel.FirstName, _registerViewModel.Preposition, _registerViewModel.LastName);
               
                // Notify the user that the account has been created.
                message = "Account succesvol aangemaakt.";
                MessageBox.Show(message, "Account aangemaakt", MessageBoxButton.OK, MessageBoxImage.Information);
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
}