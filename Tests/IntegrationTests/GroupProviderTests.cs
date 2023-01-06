using System;
using NUnit.Framework;
using Tests.Attributes;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;

namespace Tests.IntegrationTests;

[TestFixture]
public class GroupProviderTests
{
    private string _firstName = null!;
    private string _lastName = null!;
    private string _groupName = null!;
    private string _groupCode = null!;
    private string _lessonName = null!;
    private PasswordHash _hash = null!;
    private byte[] _password = null!;

    [SetUp]
    public void Setup()
    { 
        _firstName = "If you see this in the database, ";
        _lastName = "it means something went wrong while unit testing";
        _lessonName = "If you see this in the database, it means something went wrong while unit testing";
        _groupName = "If you see this in the database, it means something went wrong while unit testing";
        _groupCode = "UNITTEST";
        _hash = new PasswordHash("UnitTest");
        _password = _hash.ToArray();
    }
    
    [Test, Rollback]
    public void GroupProvider_GetById_Should_ReturnCorrectGroup_When_Called()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");

        // Act
        var groupById = new GroupProvider().GetById((int)group?["id"]!);
        
        // Assert
        Assert.AreEqual(_groupName, (string)groupById?["name"]!);
    }
    
    [Test, Rollback]
    public void GroupProvider_GetByCode_Should_ReturnCorrectGroup_When_CodeIsUNITTEST()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");

        // Act
        var groupById = new GroupProvider().GetByCode(_groupCode);
        
        // Assert
        Assert.AreEqual(_groupName, (string)groupById?["name"]!);
    }
    
    [Test, Rollback]
    public void GroupProvider_GetLessons_Should_Return2Lessons_When_GroupHas2Lessons()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");

        var lesson1 = new LessonProvider().Create(_lessonName, (int)teacher["id"]);
        var lesson2 = new LessonProvider().Create(_lessonName, (int)teacher["id"]);
        if (lesson1 == null) Assert.Fail("Lesson 2 could not be created");
        if (lesson2 == null) Assert.Fail("Lesson 2 could not be created");
        
        var lesson1Group = new LessonProvider().LinkToGroup((int)group?["id"]!, (int)lesson1?["id"]!);
        var lesson2Group = new LessonProvider().LinkToGroup((int)group?["id"]!, (int)lesson2?["id"]!);
        if (lesson1Group == null) Assert.Fail("Lesson 1 could not be linked to group");
        if (lesson2Group == null) Assert.Fail("Lesson 2 could not be linked to group");

        // Act
        var lessons = new GroupProvider().GetLessons((int)group["id"]!);
        
        // Assert
        Assert.AreEqual(2, lessons!.Count);
    }
    
    [Test, Rollback]
    public void GroupProvider_GetStudents_Should_Return2Students_When_GroupHas2Students()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");

        var student1 = new StudentProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        var student2 = new StudentProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (student1 == null) Assert.Fail("Student 1 could not be created");
        if (student2 == null) Assert.Fail("Student 2 could not be created");
        
        var student1Group = new StudentProvider().LinkToGroup((int)group?["id"]!, (int)student1?["id"]!);
        var student2Group = new StudentProvider().LinkToGroup((int)group?["id"]!, (int)student2?["id"]!);
        if (student1Group == null) Assert.Fail("Student 1 could not be linked to group");
        if (student2Group == null) Assert.Fail("Student 2 could not be linked to group");

        // Act
        var lessons = new GroupProvider().GetStudents((int)group["id"]!);
        
        // Assert
        Assert.AreEqual(2, lessons!.Count);
    }
    
    [Test, Rollback]
    public void GroupProvider_GetStudentById_Should_ReturnStudent_When_StudentIsInGroup()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");

        var student = new StudentProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (student == null) Assert.Fail("Student could not be created");
        
        var studentGroup = new StudentProvider().LinkToGroup((int)group?["id"]!, (int)student?["id"]!);
        if (studentGroup == null) Assert.Fail("Student could not be linked to group");
        
        // Act
        var linkedStudent = new GroupProvider().GetStudentById((int)group["id"], (int)student["id"]);
        
        // Assert
        Assert.NotNull(student);
    }
    
    [Test, Rollback]
    public void GroupProvider_GetStudentById_Should_NotReturnStudent_When_StudentIsNotInGroup()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");

        var student = new StudentProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (student == null) Assert.Fail("Student could not be created");
        
        // Act
        var linkedStudent = new GroupProvider().GetStudentById((int)group["id"], (int)student["id"]);
        
        // Assert
        Assert.Null(linkedStudent);
    }
    
    [Test, Rollback]
    public void GroupProvider_Create_Should_ReturnCreatedGroup_When_Called()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        // Act
        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);

        // Assert
        Assert.AreEqual(_groupName, (string)group?["name"]!);
    }
    
    [Test, Rollback]
    public void GroupProvider_UpdateGroupCode_Should_ReturnGroupWithGroupCodeUPDATED_When_NewGroupCodeIsUPDATED()
    {
        // Arrange
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");

        // Act
        var updatedGroup = new GroupProvider().UpdateGroupCode((int)group?["id"]!, "UPDATED");
        
        // Assert
        Assert.AreEqual("UPDATED", (string)updatedGroup?["code"]!);
    }
}