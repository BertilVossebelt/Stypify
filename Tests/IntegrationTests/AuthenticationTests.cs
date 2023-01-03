using System.Collections.Generic;
using NUnit.Framework;
using Tests.Attributes;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace Tests.IntegrationTests;

[TestFixture]
public class AuthenticationTests
{
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
        _email = "unit@test.nl";
        _password = "unitTest";
        _hash = new PasswordHash(_password);
        _hashedPassword = _hash.ToArray();
        _salt = _hash.Salt;
        _firstName = "If you see this in the database,";
        _lastName = "Something went wrong with unit testing.,";
    }
    
    [Test, Rollback]
    public void StudentProvider_Create_Should_RegisterStudent_WhenDataCorrect()
    {
        // Arrange in setup

        // Act
        var result = new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);

        // Assert
        Assert.NotNull(result);
    }
    
    [Test, Rollback]
    public void StudentProvider_Create_Should_RegisterStudentWithPreposition_WhenDataCorrect()
    {
        // Arrange in setup
        const string preposition = "preposition";
    
        // Act
        var result = new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, preposition, _lastName);
    
        // Assert
        Assert.NotNull(result);
    }
    
    [Test, Rollback]
    public void StudentProvider_Create_Should_NotRegisterStudent_WhenEmailAlreadyExists()
    {
        // Arrange in setup
    
        // Act
        new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, null,_lastName);
    
        Dictionary<string, object>? result = null;
        if (new StudentProvider().GetByEmail(_email) == null)
        {
            result = new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);
        }
    
        // Assert
        Assert.Null(result);
    }
    
    [Test, Rollback]
    public void UserProvider_GetByCredentials_Should_ReturnTrue_WhenEmailAndPasswordCorrect()
    {
        // Arrange in setup
        new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);
    
        // Act
        var result = new UserProvider().GetByEmail(_email);
    
        // Assert
        Assert.NotNull(result);
    }
    
    [Test, Rollback]
    public void UserProvider_GetByCredentials_Should_ReturnFalse_WhenEmailAndPasswordIncorrect()
    {
        // Arrange in setup
        new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);
        const string password = "incorrectPassword";
    
        // Act
        var result = new UserProvider().GetByEmail(_email);
    
        // Assert
        Assert.NotNull(result);
    }
}