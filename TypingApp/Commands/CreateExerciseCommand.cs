using System.Windows.Input;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class CreateExerciseCommand : CommandBase
{
    private readonly CreateExerciseViewModel _createExerciseViewModel;
    public CreateExerciseCommand(CreateExerciseViewModel createExerciseViewModel)
    {
        _createExerciseViewModel = createExerciseViewModel;
    }
    
    public override void Execute(object? parameter)
    {
        throw new System.NotImplementedException();
    }
}