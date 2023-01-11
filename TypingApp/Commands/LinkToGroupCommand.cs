using System.Windows;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

internal class LinkToGroupSaveCommand : CommandBase
{
    private readonly UserStore _userStore;
    private readonly LinkToGroupViewModel _linkToGroupViewModel;
    private int _groupId;
    private readonly NavigationService _studentDashboardNavigationService;

    public LinkToGroupSaveCommand(LinkToGroupViewModel linkToGroupViewModel, UserStore userStore,
        NavigationService studentDashboardNavigationService)
    {
        _linkToGroupViewModel = linkToGroupViewModel;
        _userStore = userStore;
        _studentDashboardNavigationService = studentDashboardNavigationService;
    }

    /*
     * Links a student to a group.
     * ----------------------------------
     * Should only be used for students.
     */
    public override void Execute(object? parameter)
    {
        // Ask for user verification.
        var linkGroupMessageBox = AskUserVerification();
        if (linkGroupMessageBox != MessageBoxResult.Yes) return;

        // Find group.
        if (!GetGroupId()) return;

        // Prevent linking if user is already linked.
        if (CheckIfUserIsLinked()) return;

        // Link to group.
        if (_userStore.Student != null)
        {
            var student = new StudentProvider().LinkToGroup(_groupId, _userStore.Student.Id);
            if (student != null) ShowLinkedMessage();
        }

        // Navigate to student dashboard.
        var navigateCommand = new NavigateCommand(_studentDashboardNavigationService);
        navigateCommand.Execute(this);
    }

    private bool CheckIfUserIsLinked()
    {
        // Check if student is already linked to group.
        if (_userStore.Student != null)
        {
            var student = new GroupProvider().GetStudentById(_groupId, _userStore.Student.Id);
            if (student == null) return false;
        }

        ShowAlreadyLinkedError();
        return true;
    }

    /*
     * Checks if group code exists and finds the group id.
     */
    private bool GetGroupId()
    {
        var group = new GroupProvider().GetByCode(_linkToGroupViewModel.GroupCode);

        // Close connection if reader doesn't have rows.
        if (group == null)
        {
            ShowGroupDoesNotExistError();
            return false;
        }

        // If reader has rows, get group id.
        _groupId = (int)group["id"];
        return true;
    }

    /*
     * Notifies the user the group code doesn't exist.
     * ---------------------------------------------
     * Show OK messagebox
     */
    private static void ShowGroupDoesNotExistError()
    {
        const string message = "Deze groep code bestaat niet";
        const MessageBoxButton type = MessageBoxButton.OK;
        const MessageBoxImage icon = MessageBoxImage.Error;

        MessageBox.Show(message, "Fout", type, icon);
    }

    /*
     * Notifies the user he is already linked to the group.
     * ---------------------------------------------
     * Show OK messagebox
     */
    private static void ShowAlreadyLinkedError()
    {
        const string message = "Je bent al gekoppeld aan deze groep";
        const MessageBoxButton type = MessageBoxButton.OK;
        const MessageBoxImage icon = MessageBoxImage.Error;

        MessageBox.Show(message, "Fout", type, icon);
    }

    /*
     * Notifies the user he is now linked to the group.
     * ---------------------------------------------
     * Show OK messagebox
     */
    private static void ShowLinkedMessage()
    {
        const string message = "Je bent gekoppeld aan de groep";
        const MessageBoxButton type = MessageBoxButton.OK;
        const MessageBoxImage icon = MessageBoxImage.Information;

        MessageBox.Show(message, "Succes", type, icon);
    }

    /*
     * Ask the user if wants to link to the group.
     * ---------------------------------------------
     * Show Yes/No messagebox
     */
    private static MessageBoxResult AskUserVerification()
    {
        const string message = "Weet je zeker dat je  aan deze groep wilt koppelen";
        const MessageBoxButton type = MessageBoxButton.YesNo;
        const MessageBoxImage icon = MessageBoxImage.Question;

        var linkGroupMessageBox = MessageBox.Show(message, "Koppelen", type, icon);
        return linkGroupMessageBox;
    }
}