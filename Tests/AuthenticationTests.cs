using System.Transactions;
using NUnit.Framework;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace Tests;

public class AuthenticationTests
{
    private TransactionScope _scope;
    private StudentProvider _studentProvider;
    private string _email;
    private string _password;
    private PasswordHash _hash;
    private byte[] _hashedPassword;
    private byte[] _salt;
    private string _firstName;
    private string _lastName;
    private UserProvider _userProvider;

    [SetUp]
    public void Setup()
    {
        _scope = new TransactionScope(); // Prevents changes to the database from being committed.
        
        _userProvider = new UserProvider();
        _studentProvider = new StudentProvider();
        _email = "unit@test.nl";
        _password = "unitTest";
        _hash = new PasswordHash(_password);
        _hashedPassword = _hash.ToArray();
        _salt = _hash.Salt;
        _firstName = "If you see this in the database,";
        _lastName = "Something went wrong with unit testing.,";
    }

    [TearDown]
    public void TearDown()
    {
        _scope.Dispose(); // Disposes the transaction scope, which rolls back the changes to the database.
    }

    [Test]
    public void StudentProvider_Create_Should_RegisterStudent_WhenDataCorrect()
    {
        // Arrange in setup
    
        // Act
        var result = _studentProvider.Create(_email, _hashedPassword, _salt, null, _firstName, _lastName);

        // Assert
        Assert.AreEqual(result, 1);
    }
    [Test]
    public void StudentProvider_Create_Should_RegisterStudentWithPreposition_WhenDataCorrect()
    {
        // Arrange in setup
        const string preposition = "preposition";

        // Act
        var result = _studentProvider.Create(_email, _hashedPassword, _salt, preposition, _firstName, _lastName);

        // Assert
        Assert.AreEqual(result, 1);
    }

    [Test]
    public void StudentProvider_Create_Should_NotRegisterStudent_WhenEmailAlreadyExists()
    {
        // Arrange in setup

        // Act
        _studentProvider.Create(_email, _hashedPassword, _salt, null, _firstName, _lastName);

        int? result = null;
        if (_studentProvider.GetByEmail(_email) == null)
        {
             result = _studentProvider.Create(_email, _hashedPassword, _salt, null, _firstName, _lastName);
        }

        // Assert
        Assert.AreEqual(result, null);
    }

    [Test]
    public void UserProvider_VerifyUser_Should_ReturnTrue_WhenEmailAndPasswordCorrect()
    {
        // Arrange in setup
        _studentProvider.Create(_email, _hashedPassword, _salt, null, _firstName, _lastName);

        // Act
        var result = _userProvider.VerifyUser(_email, _password);
        
        // Assert
        Assert.True(result);
    }

    [Test]
    public void UserProvider_VerifyUser_Should_ReturnFalse_WhenEmailAndPasswordIncorrect()
    {
        // Arrange in setup
        _studentProvider.Create(_email, _hashedPassword, _salt, null, _firstName, _lastName);
        const string password = "incorrectPassword";

        // Act
        var result = _userProvider.VerifyUser(_email, password);
        
        // Assert
        Assert.False(result);
    }
}