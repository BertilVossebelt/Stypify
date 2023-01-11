using TypingApp.Services;

namespace TypingApp.Commands;

public class NavigateCommand : CommandBase
{
    private readonly NavigationService _navigationService;
    public NavigateCommand(NavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    /*
     * Generic navigation command to navigate to a any page.
     * -----------------------------------------------------
     * Used for all actors.
     */
    public override void Execute(object? parameter)
    {
        // Use the navigation service to navigate to the page.
        _navigationService.Navigate();
    }
}