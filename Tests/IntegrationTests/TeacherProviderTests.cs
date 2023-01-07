using NUnit.Framework;
using Tests.Attributes;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Services.PasswordHash;

namespace Tests.IntegrationTests;

[TestFixture]
public class TeacherProviderTests
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
    public void TeacherProvider_GetById_Should_ReturnTeacher_When_GivenId()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        
        // Act
        var teacherById = new TeacherProvider().GetById((int)teacher?["id"]!);
        
        // Assert
        Assert.AreEqual("unit@test.nl", teacherById?["email"]!);
    }
    
    [Test, Rollback]
    public void TeacherProvider_GetGroups_Should_Return2Groups_When_TeacherHas2Groups()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group1 = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        var group2 = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group1 == null) Assert.Fail("Group 1 could not be created");
        if (group2 == null) Assert.Fail("Group 2 could not be created");

        // Act
        var groups = new TeacherProvider().GetGroups((int)teacher["id"]);
        
        // Assert
        Assert.AreEqual(2, groups!.Count);
    }
    
    [Test, Rollback]
    public void TeacherProvider_GetLessons_Should_Return2Lessons_When_TeacherHas2Lessons()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var lesson1 = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        var lesson2 = new LessonProvider().Create(_lessonName, (int)teacher?["id"]!);
        if (lesson1 == null) Assert.Fail("Lesson 1 could not be created");
        if (lesson2 == null) Assert.Fail("Lesson 2 could not be created");

        // Act
        var lessons = new TeacherProvider().GetLessons((int)teacher["id"]);
        
        // Assert
        Assert.AreEqual(2, lessons!.Count);
    }
    
    [Test, Rollback]
    public void TeacherProvider_GetByEmail_Should_ReturnTeacherWithEmailUnitTest_When_EmailIsUnitTest()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");
        
        // Act
        var teacherByEmail = new TeacherProvider().GetByEmail("unit@test.nl");
        
        // Assert
        Assert.AreEqual("unit@test.nl", teacherByEmail?["email"]!);
    }
    
    [Test, Rollback]
    public void TeacherProvider_Create_Should_ReturnTeacher_When_Called()
    {
        // Arrange in Setup
        
        // Act
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        
        // Assert
        Assert.NotNull(teacher);
    }
    
    [Test, Rollback]
    public void TeacherProvider_Create_Should_ReturnTeacherWithPreposition_When_CalledWithPreposition()
    {
        // Arrange in Setup
        const string preposition = "preposition";
        
        // Act
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, preposition, _lastName);
        
        // Assert
        Assert.AreEqual(preposition,teacher?["preposition"]);
    }
}