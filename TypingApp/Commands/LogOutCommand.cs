using TypingApp.Services;
using TypingApp.Stores;

namespace TypingApp.Commands;

public class LogOutCommand : CommandBase
{
    private UserStore _userStore;
    private NavigationService _loginNavigationService;

    public LogOutCommand(UserStore userStore, NavigationService loginNavigationService)
    {
        _userStore = userStore;
        _loginNavigationService = loginNavigationService;
    }

    /*
     * Method logs out the user and navigates to the login page.
     * ---------------------------------------------------------
     * Used for all actors.
     */
    public override void Execute(object? parameter)
    {
        // Clear the user store.
        _userStore.DeleteTeacher();
        _userStore.DeleteStudent();
        _userStore.DeleteAdmin();

        // Navigate to the login page.
        var navigateCommand = new NavigateCommand(_loginNavigationService);
        navigateCommand.Execute(this);
    }
}