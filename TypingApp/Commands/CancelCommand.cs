using System.Windows;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;
using TypingApp.Views;

namespace TypingApp.Commands;

public class CancelCommand : CommandBase
{
    private readonly NavigationStore _navigationStore;
    private readonly User _user;
    private readonly DatabaseConnection _connection;
    public CancelCommand(NavigationStore navigationStore, User user, DatabaseConnection connection)
    {
        _navigationStore = navigationStore;
        _user = user;
        _connection = connection;
    }

    public override void Execute(object? parameter)
    {
        string messageBoxText = "Weet je zeker dat je wilt annuleren";
        string caption = "Annuleren";
        MessageBoxButton button = MessageBoxButton.YesNo;
        MessageBoxImage icon = MessageBoxImage.Warning;
        MessageBoxResult result;

        result = MessageBox.Show(messageBoxText, caption, button, icon);
        if (result == MessageBoxResult.Yes)
        {
            _navigationStore.CurrentViewModel = new TeacherDashboardViewModel(_user, _navigationStore,_connection);
        }
    }
}