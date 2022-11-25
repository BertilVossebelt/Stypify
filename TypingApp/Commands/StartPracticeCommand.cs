using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class StartPracticeCommand : CommandBase
{
    private readonly User _user;
    private readonly NavigationStore _navigationStore;
    private readonly DatabaseConnection _connection;
    public StartPracticeCommand(User user, NavigationStore navigationStore, DatabaseConnection connection)
    {
        _user = user;
        _navigationStore = navigationStore;
        _connection = connection;
    }

    public override void Execute(object? parameter)
    {
        var exercise = new Exercise(_user.Characters);
        _navigationStore.CurrentViewModel = new ExerciseViewModel(exercise.Text, _navigationStore, _user, _connection);
    }
}