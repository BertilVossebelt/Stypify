using System.Windows.Input;
using TypingApp.Models;
using TypingApp.Commands;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class StudentDashboardViewModel : ViewModelBase
{
    public ICommand StartPracticeButton { get; }
    
    public StudentDashboardViewModel(User user, NavigationStore navigationStore,DatabaseConnection connection)
    { 
        StartPracticeButton = new StartPracticeCommand(user, navigationStore, connection);
    }
}