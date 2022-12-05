using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Services;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    private ICommand StartPracticeButton { get; }
    private ICommand AddToGroupButton { get; }

    public StudentDashboardViewModel(NavigationService exerciseNavigationService, NavigationService linkToGroupNavigationService)
    { 
        StartPracticeButton = new NavigateCommand(exerciseNavigationService);
        AddToGroupButton = new NavigateCommand(linkToGroupNavigationService);
    }
}