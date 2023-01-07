using System.Windows;
using TypingApp.Services;
using TypingApp.Services.DatabaseProviders;
using TypingApp.Stores;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class CreateGroupCommand : CommandBase
{
    private NavigationService _teacherDashboardNavigationService;
    private readonly UserStore _userStore;
    private readonly AddGroupViewModel _addGroupViewModel;

    public CreateGroupCommand(UserStore userStore, NavigationService teacherDashboardNavigationService,
        AddGroupViewModel addGroupViewModel)
    {
        _userStore = userStore;
        _teacherDashboardNavigationService = teacherDashboardNavigationService;
        _addGroupViewModel = addGroupViewModel;
    }
    
    /*
     * Creates a new group and adds it to the database.
     * ------------------------------------------------
     * Method should only be used for teachers.
     */
    public override void Execute(object? parameter)
    {
        string? message;

        if (!ValidateGroupData())
        {
            message = "Er is geen naam ingevoerd of geen groepscode gegenereerd."; 
            MessageBox.Show(message, "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        // Ask user for confirmation.
        message = "Weet je zeker dat je deze groep wilt opslaan";
        var saveMessageBox = MessageBox.Show(message, "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (saveMessageBox != MessageBoxResult.Yes) return;
        if(_userStore.Teacher == null) return;

        //Save to database using a provider.
        new GroupProvider().Create(_userStore.Teacher.Id, _addGroupViewModel.GroupName, _addGroupViewModel.GroupCode);
        
        // Navigate back to the teacher dashboard.
        var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
        navigateCommand.Execute(this);
    }
    
    /*
     * Validate the input from the user.
     */
    private bool ValidateGroupData()
    {
        if (_addGroupViewModel.GroupName is "" or null) return false;
        return _addGroupViewModel.GroupCode is not ("" or null);
    }
}