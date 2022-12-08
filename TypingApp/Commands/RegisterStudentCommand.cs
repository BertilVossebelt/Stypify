using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
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

        public override bool CanExecute(object? parameter)
        {
            return _registerViewModel.CanCreateAccount;
        }

        public override void Execute(object? parameter)
        {
            var password = SecureStringToString(_registerViewModel.Password);
            var passwordConfirm = SecureStringToString(_registerViewModel.PasswordConfirm);

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
                    new StudentProvider().Create(_registerViewModel.Email, _registerViewModel.Password,
                        _registerViewModel.Preposition, _registerViewModel.FirstName, _registerViewModel.LastName);

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
            var student = new StudentProvider().GetByEmail(_registerViewModel.Email);
            return student != null;
        }

        private static bool PasswordConfirmCorrect(string? password, string? passwordConfirm)
        {
            return password != null && password.Equals(passwordConfirm);
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
}