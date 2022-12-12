using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using TypingApp.Models;
using List = System.Windows.Documents.List;

namespace Tests;

public class AutoGeneratedExercisesTests
{
    private static Regex _validator { set; get; }

    [SetUp]
    public void Setup()
    {
    }

    /*
     * ====================
     * ExerciseModel tests
     * ====================
     */
    [Test]
    public void Should_ReturnExerciseWithE_When_CharactersHasOnlyE()
    {
        // Arrange
        _validator = new Regex(@"^[e ]+$");
        var characters = new List<Character> { new('e') };
        
        // Act
        var exercise = new Exercise(characters);

        // Assert
        Assert.True(_validator.IsMatch(exercise.Text));
    }

    [Test]
    public void Should_ReturnCorrectExerciseWithEANT_WHEN_CharactersAreEANT()
    {
        // Arrange
        _validator = new Regex(@"^[eant ]+$");
        var characters = new List<Character>
        {
            new('e'),
            new('a'),
            new('n'),
            new('t'),
        };
        
        // Act
        var exercise = new Exercise(characters);
        
        // Assert
        Assert.True(_validator.IsMatch(exercise.Text));
    }
}