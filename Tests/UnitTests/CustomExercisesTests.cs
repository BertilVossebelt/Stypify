using NUnit.Framework;
using TypingApp.Services;

namespace Tests.UnitTests;

[TestFixture]
public class CustomExercisesTests
{
    [SetUp]
    public void Setup()
    {
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