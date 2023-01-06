using System;
using NUnit.Framework;
using Tests.Attributes;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace Tests.IntegrationTests;

[TestFixture]
public class StudentProviderTests
{
    private string _email = null!;
    private string _password = null!;
    private PasswordHash _hash = null!;
    private byte[] _hashedPassword = null!;
    private byte[] _salt = null!;
    private string _firstName = null!;
    private string _lastName = null!;

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
    public void StudentProvider_CreateStudentStatistics_Should_GetStudentData_When_GivenStudentId()
    {
        // Arrange
        var student = new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, null, _lastName); 
        new StudentProvider().CreateStudentStatistics((int)student?["id"]!);
        
        // Act
        var result = new StudentProvider().GetStudentStatistics((int)student["id"]);

        // Assert
        Assert.AreEqual((int)result?["student_id"]!, (int)student["id"]!);
    }

    [Test, Rollback]
    public void StudentProvider_GetStudentStatistics_Should_GetStudentData_When_GivenStudentId()
    {
        // Arrange
        var student = new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, null, _lastName); 
        new StudentProvider().CreateStudentStatistics((int)student?["id"]!);
        
        // Act
        var result = new StudentProvider().GetStudentStatistics((int)student["id"]);

        // Assert
        Assert.NotNull(result);
    }
    
    [Test, Rollback]
    public void StudentProvider_UpdateStudentStatistics_Should_UpdateCompletedExercises_When_GivenStudentId()
    {
        // Arrange
        var student = new StudentProvider().Create(_email, _hashedPassword, _salt, _firstName, null, _lastName);
        var studentStatistics = new StudentProvider().CreateStudentStatistics((int)student?["id"]!);
        var completedExercises = (int)(studentStatistics?["completed_exercises"] ?? 0) + 1;

        // Act
        var result = new StudentProvider().UpdateStudentStatistics((int)student["id"], completedExercises);
        
        // Assert
        Assert.AreEqual((int)result?["completed_exercises"]!, completedExercises);
    }
}