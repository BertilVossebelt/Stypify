using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class StartPracticeCommand : CommandBase
{
    private readonly User _user;
    private readonly NavigationStore _navigationStore;
    public StartPracticeCommand(User user, NavigationStore navigationStore)
    {
        _user = user;
        _navigationStore = navigationStore;
    }

    public override void Execute(object? parameter)
    {
        var exercise = new Exercise(_user.Characters);
        _navigationStore.CurrentViewModel = new ExerciseViewModel(exercise.Text, _navigationStore, _user);
    }
}