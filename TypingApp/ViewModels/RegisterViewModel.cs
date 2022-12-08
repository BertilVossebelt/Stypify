using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;

namespace TypingApp.ViewModels;

public class RegisterViewModel : ViewModelBase, INotifyDataErrorInfo
{
    // Properties
    private string _email;
    private string _firstName;
    private string? _preposition;
    private string _lastName;
    private SecureString _password;
    private SecureString _passwordConfirm;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
            ClearErrors(nameof(Email));

            CheckEmailErrors();

            OnPropertyChanged(nameof(CanCreateAccount));
        }
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged();
            ClearErrors(nameof(FirstName));

            CheckFirstNameErrors();

            OnPropertyChanged(nameof(CanCreateAccount));
        }
    }

    public string? Preposition
    {
        get => _preposition;
        set
        {
            _preposition = value;
            OnPropertyChanged();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged();
            ClearErrors(nameof(LastName));

            CheckLastNameErrors();

            OnPropertyChanged(nameof(CanCreateAccount));
        }
    }

    public SecureString Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            ClearErrors(nameof(Password));
            ClearErrors(nameof(PasswordConfirm));

            CheckPasswordErrors();

            OnPropertyChanged(nameof(CanCreateAccount));
        }
    }

    public SecureString PasswordConfirm
    {
        get => _passwordConfirm;
        set
        {
            _passwordConfirm = value;
            OnPropertyChanged();
            ClearErrors(nameof(Password));
            ClearErrors(nameof(PasswordConfirm));

            CheckPasswordErrors();

            OnPropertyChanged(nameof(CanCreateAccount));
        }
    }

    // Commands
    public ICommand GoToLoginButton { get; }
    public ICommand RegisterButton { get; }

    // Constructor
    public RegisterViewModel(NavigationService loginNavigationService, DatabaseConnection connection)
    {
        _connection = connection;
        GoToLoginButton = new NavigateCommand(loginNavigationService);
        RegisterButton = new RegisterStudentCommand(this, connection);

        _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
    }
    
    // Events voor data validation
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }

    // Data validation
    private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;
    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;

            OnPropertyChanged();
            OnPropertyChanged(nameof(HasErrorMessage));
        }
    }
    
    public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasErrors => _propertyNameToErrorsDictionary.Any();
    public bool CanCreateAccount => !HasErrors;
    
    public IEnumerable GetErrors(string? propertyName)
    {
        return _propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
    }

    // Clear de errors om te checken of de data nu wel correct is.
    private void ClearErrors(string propertyName)
    {
        _propertyNameToErrorsDictionary.Remove(propertyName);
        ErrorMessage = null;
        OnErrorsChanged(propertyName);
    }
    
    // Voeg een error toe aan de dictionary om dit bij te houden.
    private void AddError(string errorMessage, string propertyName)
    {
        if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            _propertyNameToErrorsDictionary.Add(propertyName, new List<string>());

        _propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
        ErrorMessage = errorMessage;
        OnErrorsChanged(propertyName);
    }

    // Check of er errors zijn die de email kan hebben.
    private void CheckEmailErrors()
    {
        if (string.IsNullOrWhiteSpace(Email))
            AddError("Email mag niet leeg zijn.", nameof(Email));
        else if (Email.Contains(' '))
            AddError("Email mag geen spaties bevatten.", nameof(Email));
        else if (!isValidEmail(Email)) AddError("Voer een correct emailadres in.", nameof(Email));
    }

    // Check of er errors zijn die de voornaam kan hebben.
    private void CheckFirstNameErrors()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            AddError("Voornaam mag niet leeg zijn.", nameof(FirstName));
        else if (!Regex.IsMatch(FirstName, @"^[a-zA-Z]+$"))
            AddError("Voornaam mag alleen letters bevatten.", nameof(FirstName));
    }

    // Check of er errors zijn die de achternaam kan hebben.
    private void CheckLastNameErrors()
    {
        if (string.IsNullOrWhiteSpace(LastName))
            AddError("Achternaam mag niet leeg zijn.", nameof(LastName));
        else if (!Regex.IsMatch(LastName, @"^[a-zA-Z]+$"))
            AddError("Achternaam mag alleen letters bevatten.", nameof(LastName));
    }

    //Check of er errors zijn die het wachtwoord kan hebben.
    private void CheckPasswordErrors()
    {
        if (!PasswordConfirmCorrect(Password, PasswordConfirm))
        {
            AddError("Wachtwoorden moeten gelijk zijn", nameof(Password));
            AddError("Wachtwoorden moeten gelijk zijn", nameof(PasswordConfirm));
        }
        else if (Password.Length == 0)
            AddError("Wachtwoord mag niet leeg zijn.", nameof(Password));
        else if (Password.Length < 6)
            AddError("Wachtwoord moet minimaal 6 karakters bevatten.", nameof(Password));
    }

    // Check of er een correct emailadres gebruikt wordt (niet strict).
    private bool isValidEmail(string email)
    {
        var pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" +
                      @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" +
                      @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }

    // Check of de twee ingevoerde wachtwoorden gelijk zijn.
    private bool PasswordConfirmCorrect(SecureString password, SecureString passwordConfirm)
    {
        var bstr1 = IntPtr.Zero;
        var bstr2 = IntPtr.Zero;
        try
        {
            bstr1 = Marshal.SecureStringToBSTR(password);
            bstr2 = Marshal.SecureStringToBSTR(passwordConfirm);
            var length1 = Marshal.ReadInt32(bstr1, -4);
            var length2 = Marshal.ReadInt32(bstr2, -4);
            if (length1 == length2)
                for (var x = 0; x < length1; ++x)
                {
                    var b1 = Marshal.ReadByte(bstr1, x);
                    var b2 = Marshal.ReadByte(bstr2, x);
                    if (b1 != b2) return false;
                }
            else return false;

            return true;
        }
        finally
        {
            if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
            if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
        }
    }
}