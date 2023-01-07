using NUnit.Framework;
using Tests.Attributes;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Services.PasswordHash;

namespace Tests.IntegrationTests;

[TestFixture]
public class UserProviderTests
{
    private string _firstName = null!;
    private string _lastName = null!;
    private PasswordHash _hash = null!;
    private byte[] _password = null!;

    [SetUp]
    public void Setup()
    {
        _firstName = "If you see this in the database, ";
        _lastName = "it means something went wrong while unit testing";
        _hash = new PasswordHash("UnitTest");
        _password = _hash.ToArray();
    }

    [Test, Rollback]
    public void UserProvider_GetById_Should_ReturnUser_When_GivenId()
    {
        // Arrange
        var user = new StudentProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (user == null) Assert.Fail("User could not be created");

        // Act
        var userById = new UserProvider().GetById((int)user?["id"]!);

        // Assert
        Assert.AreEqual("unit@test.nl", userById?["email"]!);

    }

    [Test, Rollback]
    public void UserProvider_GetByEmail_Should_ReturnUser_When_GivenEmail()
    {
        // Arrange
        var user = new StudentProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (user == null) Assert.Fail("User could not be created");

        // Act
        var userByEmail = new UserProvider().GetByEmail((string)user?["email"]!);

        // Assert
        Assert.AreEqual("unit@test.nl", userByEmail?["email"]!);
    }
}