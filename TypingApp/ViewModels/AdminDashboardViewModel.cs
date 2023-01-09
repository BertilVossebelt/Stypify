using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class AdminDashboardViewModel : ViewModelBase, INotifyDataErrorInfo
{
    private readonly UserStore _userStore;
    
    // Properties
    private string _email;
    private string _firstName;
    private string? _preposition;
    private string _lastName;
    private SecureString _password;
    private SecureString _passwordConfirm;
    private string _deleteEmail = "Vul email in om te verwijderen";
    
    private ObservableCollection<Teacher> _teachers;

    public ObservableCollection<Teacher> Teachers
    {
        get => _teachers;
        set
        {
            _teachers = value;
            OnPropertyChanged();
        }
    }
    
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

    public string DeleteEmail
    {
        get => _deleteEmail;
        set
        {
            _deleteEmail = value;
            OnPropertyChanged();
        }
    }
    
    // Commands
    public string WelcomeMessage { get; set; }
    public ICommand DeleteTeacherButton { get; }
    public ICommand RegisterTeacherButton { get; }
    public ICommand LogOutButton { get; }

    // Constructor
    public AdminDashboardViewModel(UserStore userStore, NavigationService loginNavigationService)
    {
        _userStore = userStore;
        WelcomeMessage = GetName();
        Teachers = new ObservableCollection<Teacher>();
        GetTeachers();
        
        RegisterTeacherButton = new RegisterTeacherCommand(this);
        LogOutButton = new LogOutCommand(userStore, loginNavigationService);
        DeleteTeacherButton = new DeleteTeacherCommand(this);
        
        _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();
    }

    // Get all teacher accounts.
    private void GetTeachers()
    {
        var teachers = new TeacherProvider().GetAll();
        if (teachers == null) return;
        foreach (var teacher in teachers) 
        {
            Teachers.Add(new Teacher(teacher)); 
        }
    }
    
    // Get name for welcome message.
    private string GetName()
    {
        if (_userStore.Admin?.Preposition != null)
        {
            return $"Welkom {_userStore.Admin.FirstName} {_userStore.Admin.Preposition} {_userStore.Admin.LastName}";
        }

        return _userStore.Admin?.Preposition == null
            ? $"Welkom {_userStore.Admin?.FirstName} {_userStore.Admin?.LastName}"
            : "Error, admin = Null";
    }

    // Events for data validation
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

    // Clear the errors to check if the data is correct this time.
    private void ClearErrors(string propertyName)
    {
        _propertyNameToErrorsDictionary.Remove(propertyName);
        ErrorMessage = null;
        OnErrorsChanged(propertyName);
    }

    // Add an error to the dictionary.
    private void AddError(string errorMessage, string propertyName)
    {
        if (!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            _propertyNameToErrorsDictionary.Add(propertyName, new List<string>());

        _propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
        ErrorMessage = errorMessage;
        OnErrorsChanged(propertyName);
    }

    // Validate Email.
    private void CheckEmailErrors()
    {
        if (string.IsNullOrWhiteSpace(Email))
            AddError("Email mag niet leeg zijn.", nameof(Email));
        else if (Email.Contains(' '))
            AddError("Email mag geen spaties bevatten.", nameof(Email));
        else if (!isValidEmail(Email)) AddError("Voer een correct emailadres in.", nameof(Email));
    }

    // Validate firstName.
    private void CheckFirstNameErrors()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            AddError("Voornaam mag niet leeg zijn.", nameof(FirstName));
        else if (!Regex.IsMatch(FirstName, @"^[a-zA-Z]+$"))
            AddError("Voornaam mag alleen letters bevatten.", nameof(FirstName));
    }

    // Validate lastName.
    private void CheckLastNameErrors()
    {
        if (string.IsNullOrWhiteSpace(LastName))
            AddError("Achternaam mag niet leeg zijn.", nameof(LastName));
        else if (!Regex.IsMatch(LastName, @"^[a-zA-Z]+$"))
            AddError("Achternaam mag alleen letters bevatten.", nameof(LastName));
    }

    // Validate Password.
    private void CheckPasswordErrors()
    {
        if (!PasswordConfirmCorrect(Password, PasswordConfirm))
        {
            AddError("Wachtwoorden moeten gelijk zijn", nameof(Password));
            AddError("Wachtwoorden moeten gelijk zijn", nameof(PasswordConfirm));
        }
        else if (Password.Length == 0)
        {
            AddError("Wachtwoord mag niet leeg zijn.", nameof(Password));
        }
        else if (Password.Length < 6)
        {
            AddError("Wachtwoord moet minimaal 6 karakters bevatten.", nameof(Password));
        }
    }

    // Validate Email (not strict).
    private bool isValidEmail(string email)
    {
        var pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" +
                      @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" +
                      @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }

    // Validate Control password.
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