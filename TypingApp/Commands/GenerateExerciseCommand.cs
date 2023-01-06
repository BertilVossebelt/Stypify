using System.Collections.Generic;
using TypingApp.Models;
using TypingApp.Stores;

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

    /*
     * Created a new exercise with a random character from the list of characters.
     * ---------------------------------------------------------------------------
     * Method is used for students.
     */
    public override void Execute(object? parameter)
    {
        var text = new Exercise(_characters).Text;
        _exerciseStore.CreateExercise(text);
    }
}