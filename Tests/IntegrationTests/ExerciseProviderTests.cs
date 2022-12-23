using NUnit.Framework;
using Tests.Attributes;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace Tests.IntegrationTests;

[TestFixture]
public class ExerciseProviderTests
{
    private TeacherProvider _teacherProvider = null!;
    private ExerciseProvider _exerciseProvider = null!;

    
    [SetUp]
    public void Setup()
    {
        _teacherProvider = new TeacherProvider();
        _exerciseProvider = new ExerciseProvider();
    }
    
    [Test, Rollback]
    public void ExerciseProvider_Create_Should_ReturnCreatedExercise_When_Called()
    {
            // Arrange
            const string firstName = "If you see this in the database, ";
            const string lastName = "it means something went wrong while unit testing";
            var hash = new PasswordHash("UnitTest");
            var password = hash.ToArray();

            var teacher = _teacherProvider.Create("unit@test.nl", password, hash.Salt, firstName, null, lastName);
            if (teacher == null) Assert.Fail("Teacher could not be created");

            // Act
            var exercise = _exerciseProvider.Create((int)teacher?["id"], "UnitTest", "UnitTest");

            // Assert
            Assert.AreEqual("UnitTest", exercise?["name"]);
    }

    [Test, Rollback]
    public void ExerciseProvider_GetById_Should_ReturnCorrectExercise_When_GivenId()
    {
        // Arrange
        const string firstName = "If you see this in the database, ";
        const string lastName = "it means something went wrong while unit testing";
        var hash = new PasswordHash("UnitTest");
        var password = hash.ToArray();

        var teacher = _teacherProvider.Create("unit@test.nl", password, hash.Salt, firstName, null, lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        const string exerciseName = "UnitTest";
        const string exerciseText =
            "If you see this exercise in the database, it means something went wrong while unit testing";
        var newExercise = _exerciseProvider.Create((int)teacher?["id"], exerciseName, exerciseText);
        if (newExercise == null) Assert.Fail("Exercise could not be created");

        // Act
        var exercise = _exerciseProvider.GetById((int)newExercise["id"]);

        // Assert
        Assert.AreEqual("UnitTest", exercise?["name"]);
    }

    [Test, Rollback]
    public void ExerciseProvider_GetAll_Should_ReturnAllExercises_When_GivenTeacherId()
    {
        // Arrange
        const string firstName = "If you see this in the database, ";
        const string lastName = "it means something went wrong while unit testing";
        var hash = new PasswordHash("UnitTest");
        var password = hash.ToArray();

        var teacher = _teacherProvider.Create("unit@test.nl", password, hash.Salt, firstName, null, lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        const string exerciseName = "UnitTest";
        const string exerciseText =
            "If you see this exercise in the database, it means something went wrong while unit testing";
        var newExercise = _exerciseProvider.Create((int)teacher?["id"], exerciseName, exerciseText);
        if (newExercise == null) Assert.Fail("Exercise could not be created");

        // Act
        var exercises = _exerciseProvider.GetAll((int)teacher?["id"]);

        // Assert
        Assert.AreEqual(1, exercises.Count);
    }
}