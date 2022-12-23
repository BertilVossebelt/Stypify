using System;
using System.Collections.Generic;
using System.Transactions;
using NUnit.Framework;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;

namespace Tests;

public class CustomExercisesTests
{
    private TransactionScope _scope = null!;

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
    public void ExerciseProvider_Create_Should_StoreCustomExerciseInDatabase_When_ExerciseCreated()
    {
        // Arrange
        const string firstName = "If you see this in the database, ";
        const string lastName = "it means something went wrong while unit testing";
        
        var hash = new PasswordHash("UnitTest");
        var password = hash.ToArray();

        var teacher = new TeacherProvider().Create("unit@test.nl", password, hash.Salt, firstName, null, lastName);
        
        const string exerciseText = "If you see this exercise in the database, it means something went wrong while unit testing";
        const string exerciseName = "UnitTest";
        Dictionary<string, object>? exercise = null;

        Console.WriteLine(teacher);
        
        // Act
        if (teacher != null)
        {
            exercise = new ExerciseProvider().Create((int)teacher["id"], exerciseName, exerciseText);
        }

        // Assert
        Assert.IsNotNull(exercise);
    }

    [Test]
    public void AuditExerciseService_Audit_Should_MarkMissingB_When_InputAExpectedAB()
    {
        // Arrange
        var auditExerciseService = new AuditExerciseService();
        const string expected = "AB";
        const string input = "A";
        
        // Act
        var correctedExercise = auditExerciseService.Audit(input, expected);

        // Assert
        foreach (var character in correctedExercise)
        {
            if(character is { Char: 'B', Missing: true }) Assert.Pass();
        }
        
        Assert.Fail();
    }
    
    [Test]
    public void AuditExerciseService_Audit_Should_MarkExtraB_When_InputABExpectedA()
    {
        // Arrange
        var auditExerciseService = new AuditExerciseService();
        const string expected = "A";
        const string input = "AB";
        
        // Act
        var correctedExercise = auditExerciseService.Audit(input, expected);

        // Assert
        foreach (var character in correctedExercise)
        {
            if(character is { Char: 'B', Extra: true }) Assert.Pass();
        }
        
        Assert.Fail();
    }
}