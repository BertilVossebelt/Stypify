using System.Windows;
using TypingApp.Services;

namespace TypingApp.Commands;

public class CancelCommand : CommandBase
{
    private readonly NavigationService _teacherDashboardNavigationService;

    public CancelCommand(NavigationService teacherDashboardNavigationService)
    {
        _teacherDashboardNavigationService = teacherDashboardNavigationService;
    }

    public override void Execute(object? parameter)
    {
        var cancelMessageBox = MessageBox.Show("Weet je zeker dat je wilt annuleren", "Annuleren", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        
        if (cancelMessageBox != MessageBoxResult.Yes) return;
        var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
        navigateCommand.Execute(this);
    }
}