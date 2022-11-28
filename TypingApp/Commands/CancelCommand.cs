using System.Windows;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;
using TypingApp.ViewModels;
using TypingApp.Views;

namespace TypingApp.Commands;

public class CancelCommand : CommandBase
{
    private readonly NavigationService _teacherDashboardNavigationService;
    private readonly User _user;
    private readonly DatabaseConnection _connection;

    public CancelCommand(NavigationService teacherDashboardNavigationService, User user, DatabaseConnection connection)
    {
        _teacherDashboardNavigationService = teacherDashboardNavigationService;
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
            var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
            navigateCommand.Execute(this);
        }
    }
}