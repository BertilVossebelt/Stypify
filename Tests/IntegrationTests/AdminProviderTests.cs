using System.Transactions;
using NUnit.Framework;
using Tests.Attributes;
using TypingApp.Services.DatabaseProviders;

namespace Tests;

[TestFixture]
public class AdminProviderTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test, Rollback]
    public void AdminProvider_GetById_Should_FetchCorrectAdminAccount_When_GivenId()
    {
        // Arrange

        /*This should be an admin account, but this is reliant on the database state,
         given that there are no functions to create admin accounts.*/
        const int id = 92;

        // Act
        var admin = new AdminProvider().GetById(id);
        
        // Assert
        Assert.NotNull(admin);
    }
}