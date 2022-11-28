using System;
using System.Collections.Generic;
using System.Linq;
using TypingApp.Models;
using TypingApp.Stores;
using NavigationService = TypingApp.Services.NavigationService;

namespace TypingApp.Commands;

public class GenerateExerciseCommand : CommandBase
{
    private readonly List<Character> _characters;
    private readonly ExerciseStore _exerciseStore;
    public GenerateExerciseCommand(ExerciseStore exerciseStore, List<Character> characters)
    {
        _exerciseStore = exerciseStore;
        _characters = characters;
    }

    public override void Execute(object? parameter)
    {
        var text = new Exercise(_characters).Text;
        _exerciseStore.CreateExercise(text);
    }
}