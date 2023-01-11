using NUnit.Framework;
using TypingApp.Services;
using Tests.Attributes;
using TypingApp.Models;
using TypingApp.Services.DatabaseProviders;
using System.Security.Policy;
using TypingApp.Services.PasswordHash;

namespace Tests.UnitTests;

[TestFixture]
public class GroupCodeServiceTests
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
    public void GroupCodeService_GenerateCode_Should_Return8NumberCode_When_Generated()
    {
        // Arrange
        var groupCodeService = new GroupCodeService();

        // Act
        var groupCode = groupCodeService.GenerateCode();

        // Assert
        Assert.AreEqual(8, groupCode.Length);
    }

    [Test, Rollback]
    public void GroupCodeService_GenerateCode_Should_BeUnique_When_Generated()
    {
        // Arrange
        var groupCodeService = new GroupCodeService();

        // Act
        var groupCode1 = groupCodeService.GenerateCode();
        var groupCode2 = groupCodeService.GenerateCode();

        // Assert
        Assert.AreNotEqual(groupCode1,groupCode2);
    }

    [Test, Rollback]
    public void GroupCodeService_UpdateCodeInDatabase_Should_UpdateCodeInDataBase_When_GivenIdAndNewCode()
    {
        // Arrange
        var groupCodeService = new GroupCodeService();
        var teacher = new TeacherProvider().Create("unit@test.nl", _password, _hash.Salt, _firstName, null, _lastName);
        if (teacher == null) Assert.Fail("Teacher could not be created");

        var group = new GroupProvider().Create((int)teacher?["id"]!, _groupName, _groupCode);
        if (group == null) Assert.Fail("Group could not be created");

        // Act
        groupCodeService.UpdateCodeInDatabase((int)group?["id"]!, "UPDATED");

        var updatedGroup = new GroupProvider().GetById((int)group?["id"]!);
        // Assert
        Assert.AreEqual("UPDATED", (string)updatedGroup?["code"]!);
    }
}
