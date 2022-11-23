using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class TeacherDashboardViewModel : ViewModelBase
{
    public ICommand AddGroupButton { get; }

    public TeacherDashboardViewModel(User user, NavigationStore navigationStore)
    {
        AddGroupButton = new AddGroupCommand(user, navigationStore);
    }
}
