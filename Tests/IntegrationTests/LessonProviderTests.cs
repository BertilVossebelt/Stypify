using NUnit.Framework;
using System;
using Tests.Attributes;

using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace Tests.IntegrationTests;

[TestFixture]
public class LessonProviderTests
{
    private string _firstName = null!;
    private string _lastName = null!;
    private PasswordHash _hash = null!;
    private byte[] _password = null!;
    private string _groupName = null!;
    private string _groupCode = null!;
    private string _lessonName = null!;

    [SetUp]
    public void Setup()
    {
        _firstName = "If you see this in the database, ";
        _lastName = "it means something went wrong while unit testing";
        _groupName = "If you see this in the database, it means something went wrong while unit testing";
        _lessonName = "If you see this in the database, it means something went wrong while unit testing";
        _groupCode = "UnitTest";
        _hash = new PasswordHash("UnitTest");
        _password = _hash.ToArray();
    }

    
    [Test, Rollback]
    public void LessonProvider_GetById_Should_GetLessonById_When_GivenLessonId()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        var lesson = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        if (lesson == null) Assert.Fail("Lesson could not be created");

        // Act
        var lessonrById = new LessonProvider().GetById((int)lesson?["id"]!);

        // Assert
        Assert.AreEqual(_lessonName, lessonrById?["name"]!);
    }
    [Test, Rollback]
    public void LessonProvider_GetExercises_Should_GetExercisesFromLesson_When_GivenLessonId()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        var lesson = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        if (lesson == null) Assert.Fail("Lesson could not be created");

        var exercise1 = new ExerciseProvider().Create((int)teacher?["id"]!, "UnitTest", "UnitTest");
        if (exercise1 == null) Assert.Fail("Exercise could not be created");
        var LessonExerciseLink1 = new ExerciseProvider().LinkToLesson((int)lesson?["id"]!, (int)exercise1?["id"]!);
        if (LessonExerciseLink1 == null) Assert.Fail("Link could not be created");

        var exercise2 = new ExerciseProvider().Create((int)teacher?["id"]!, "UnitTest", "UnitTest");
        if (exercise2 == null) Assert.Fail("Exercise could not be created");
        var LessonExerciseLink2 = new ExerciseProvider().LinkToLesson((int)lesson?["id"]!, (int)exercise2?["id"]!);
        if (LessonExerciseLink2 == null) Assert.Fail("Link could not be created");

        // Act
        var exercises = new LessonProvider().GetExercises((int)lesson?["id"]!);

        // Assert
        Assert.AreEqual((int)exercise1["id"], (int)exercises?[0]["id"]!);
        Assert.AreEqual((int)exercise2["id"], (int)exercises?[1]["id"]!);
    }
    [Test, Rollback]
    public void LessonProvider_Create_Should_CreateAndGetLesson_When_GivenLessonNameAndTeacherID()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
       
        // Act
        var lesson = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);

        // Assert
        Assert.AreEqual(_lessonName, lesson?["name"]!);
    }
    [Test, Rollback]
    public void LessonProvider_LinkToGroup_Should_LinkLessonToGroupAndGetGroupLessonLink_When_GivenLessonIdAndGroupId()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        var lesson = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        if (lesson == null) Assert.Fail("Lesson could not be created");
        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");
   
        // Act
        var Group_lessonLink = new LessonProvider().LinkToGroup((int)group?["id"]!, (int)lesson?["id"]!);

        // Assert
        Assert.IsNotNull(Group_lessonLink);

    }
    [Test, Rollback]
    public void LessonProvider_GetLinkedGroups_Should_GetLinkedGroupsOfLesson_When_GivenLessonId()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        var lesson = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        if (lesson == null) Assert.Fail("Lesson could not be created");

        var group1 = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group1 == null) Assert.Fail("Group could not be created");
        var group_lessonLink1 = new LessonProvider().LinkToGroup((int)group1?["id"]!, (int)lesson?["id"]!);
        if (group_lessonLink1 == null) Assert.Fail("group_lessonLink could not be created");

        var group2 = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group2 == null) Assert.Fail("Group could not be created");
        var group_lessonLink2 = new LessonProvider().LinkToGroup((int)group2?["id"]!, (int)lesson?["id"]!);
        if (group_lessonLink2 == null) Assert.Fail("group_lessonLink could not be created");

        // Act
        var linkedGroups = new LessonProvider().GetLinkedGroups((int)lesson?["id"]!);

        // Assert
        Assert.AreEqual(linkedGroups!.Count, 2);
        Assert.AreEqual(linkedGroups![0]["id"], (int)group1?["id"]!);
        Assert.AreEqual(linkedGroups![1]["id"], (int)group2?["id"]!);
    }
    [Test, Rollback]
    public void LessonProvider_DeleteLinksToGroups_Should_DeleteLinksAllFromGroupToLesson_When_GivenLessonId()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        var lesson = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        if (lesson == null) Assert.Fail("Lesson could not be created");

        var group1 = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group1 == null) Assert.Fail("Group could not be created");
        var group_lessonLink1 = new LessonProvider().LinkToGroup((int)group1?["id"]!, (int)lesson?["id"]!);
        if (group_lessonLink1 == null) Assert.Fail("group_lessonLink could not be created");

        var group2 = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group2 == null) Assert.Fail("Group could not be created");
        var group_lessonLink2 = new LessonProvider().LinkToGroup((int)group2?["id"]!, (int)lesson?["id"]!);
        if (group_lessonLink2 == null) Assert.Fail("group_lessonLink could not be created");

        // Act
        var delete = new LessonProvider().DeleteLinksToGroups((int)lesson?["id"]!);

        // Assert
        var linkedGroups = new LessonProvider().GetLinkedGroups((int)lesson?["id"]!);
        Assert.IsNull(linkedGroups);
    }
    [Test, Rollback]
    public void LessonProvider_DeleteLinksToExercises_Should_DeleteLinksAllFromExerciseToLesson_When_GivenLessonId()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        var lesson = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        if (lesson == null) Assert.Fail("Lesson could not be created");

        var exercise1 = new ExerciseProvider().Create((int)teacher?["id"]!, "UnitTest", "UnitTest");
        if (exercise1 == null) Assert.Fail("Exercise could not be created");
        var LessonExerciseLink1 = new ExerciseProvider().LinkToLesson((int)lesson?["id"]!, (int)exercise1?["id"]!);
        if (LessonExerciseLink1 == null) Assert.Fail("Link could not be created");

        var exercise2 = new ExerciseProvider().Create((int)teacher?["id"]!, "UnitTest", "UnitTest");
        if (exercise2 == null) Assert.Fail("Exercise could not be created");
        var LessonExerciseLink2 = new ExerciseProvider().LinkToLesson((int)lesson?["id"]!, (int)exercise2?["id"]!);
        if (LessonExerciseLink2 == null) Assert.Fail("Link could not be created");

        // Act
        new LessonProvider().DeleteLinksToExercises((int)lesson?["id"]!);

        // Assert
        var linkedGroups = new LessonProvider().GetExercises((int)lesson?["id"]!);
        Assert.IsNull(linkedGroups);
    }
    [Test, Rollback]
    public void LessonProvider_UpdateLesson_Should_UpdateAndGetLesson_When_GivenLessonIdAndNameAndTeacherID()
    {
        var newName = "newName";
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        var lesson = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        if (lesson == null) Assert.Fail("Lesson could not be created");
        // Act
        var update = new LessonProvider().UpdateLesson((int)lesson?["id"]!, newName, (int)teacher?["id"]!);

        // Assert
        Assert.AreEqual(newName, update?["name"]!);
    }
}