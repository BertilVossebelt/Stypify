using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;
using TypingApp.Views;

namespace TypingApp.Commands;

public class BackCommand : CommandBase
{
    private readonly NavigationStore _navigationStore;
    private readonly User _user;
    private readonly DatabaseConnection _connection;
    public BackCommand(NavigationStore navigationStore, User user, DatabaseConnection connection)
    {
        _navigationStore = navigationStore;
        _user = user;
        _connection = connection;
    }

    public override void Execute(object? parameter)
    {
        if (_user.IsTeacher)
        {
            _navigationStore.CurrentViewModel = new TeacherDashboardViewModel(_user, _navigationStore,_connection);
        }
        else
        {
            _navigationStore.CurrentViewModel = new StudentDashboardViewModel(_user, _navigationStore,_connection);
        }
        
    }
}