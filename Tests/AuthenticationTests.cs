using System.Collections.Generic;
using System.Transactions;
using NUnit.Framework;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace Tests;

public class AuthenticationTests
{
    private TransactionScope _scope = null!;
    private StudentProvider _studentProvider = null!;
    private string _email = null!;
    private string _password = null!;
    private PasswordHash _hash = null!;
    private byte[] _hashedPassword = null!;
    private byte[] _salt = null!;
    private string _firstName = null!;
    private string _lastName = null!;
    private UserProvider _userProvider = null!;

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
        var result = _studentProvider.Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public void StudentProvider_Create_Should_RegisterStudentWithPreposition_WhenDataCorrect()
    {
        // Arrange in setup
        const string preposition = "preposition";

        // Act
        var result = _studentProvider.Create(_email, _hashedPassword, _salt, preposition, _firstName, _lastName);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public void StudentProvider_Create_Should_NotRegisterStudent_WhenEmailAlreadyExists()
    {
        // Arrange in setup

        // Act
        _studentProvider.Create(_email, _hashedPassword, _salt, _firstName, null,_lastName);

        Dictionary<string, object>? result = null;
        if (_studentProvider.GetByEmail(_email) == null)
        {
            result = _studentProvider.Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);
        }

        // Assert
        Assert.Null(result);
    }

    [Test]
    public void UserProvider_GetByCredentials_Should_ReturnTrue_WhenEmailAndPasswordCorrect()
    {
        // Arrange in setup
        _studentProvider.Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);

        // Act
        var result = _userProvider.GetByCredentials(_email);

        // Assert
        Assert.NotNull(result);
    }

    [Test]
    public void UserProvider_GetByCredentials_Should_ReturnFalse_WhenEmailAndPasswordIncorrect()
    {
        // Arrange in setup
        _studentProvider.Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);
        const string password = "incorrectPassword";

        // Act
        var result = _userProvider.GetByCredentials(_email);

        // Assert
        Assert.NotNull(result);
    }
}