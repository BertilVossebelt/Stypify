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

    public CancelCommand(NavigationService teacherDashboardNavigationService)
    {
        _teacherDashboardNavigationService = teacherDashboardNavigationService;
    }

    public override void Execute(object? parameter)
    {
        var CancelMessageBox = MessageBox.Show("Weet je zeker dat je wilt annuleren", "Annuleren", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (CancelMessageBox == MessageBoxResult.Yes)
        {
            var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
            navigateCommand.Execute(this);
        }
    }
}