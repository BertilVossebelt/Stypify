using System.Windows;
using TypingApp.Services;

namespace TypingApp.Commands;

public class CancelCommand : CommandBase
{
    private readonly NavigationService _navigationService;

    public CancelCommand(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    /*
     * Asks the user if they want to cancel the current operation.
     * -----------------------------------------------------------
     * Used for all actors.
     */
    public override void Execute(object? parameter)
    {
        // Ask for confirmation.
        const string message = "Weet je zeker dat je deze actie wilt annuleren?";
        var cancelMessageBox = MessageBox.Show(message, "Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (cancelMessageBox != MessageBoxResult.Yes) return;
        
        // Cancel the current operation if the user confirms.
        var navigateCommand = new NavigateCommand(_navigationService);
        navigateCommand.Execute(this);
    }
}