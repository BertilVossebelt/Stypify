using System.Collections.Generic;
using System.Transactions;
using NUnit.Framework;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace Tests;

public class CustomExercisesTests
{
    private TransactionScope _scope;

    [SetUp]
    public void Setup()
    {
        _scope = new TransactionScope();
    }

    [TearDown]
    public void TearDown()
    {
        _scope.Dispose();
    }

    [Test]
    public void Should_StoreCustomExerciseInDatabase_When_ExerciseCreated()
    {
        // Arrange
        const string firstName = "If you see this in the database, ";
        const string lastName = "it means something went wrong while unit testing"; 
        var hash = new PasswordHash("UnitTest");
        var password = hash.ToArray();
        var teacher = new TeacherProvider().Create("unit@test.nl", password, firstName, lastName);
        
        const string exerciseText = "If you see this exercise in the database, it means something went wrong while unit testing";
        const string exerciseName = "UnitTest";
        Dictionary<string, object>? exercise = null;
        
        // Act
        if (teacher != null)
        {
            exercise = new ExerciseProvider().Create((int)teacher["id"], exerciseName, exerciseText);
        }
    
        // Assert
        Assert.IsNotNull(exercise);
    }
}