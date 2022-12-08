using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class AddGroupViewModel : ViewModelBase
{
    public ICommand BackButton { get; }
    public ICommand CancelButton { get; }
    public ICommand SaveButton { get; }
    public ICommand NewGroupCodeButton { get; }
    private Group _group;
    private string? _groupCodeText;
    private string? _groupNameText;

    public string GroupCodeText
    {
        get => _groupCodeText;
        set { _groupCodeText = value; OnPropertyChanged(); }
    }

    public string GroupNameText
    {
        get => _groupNameText;
        set{ _groupNameText = value; _group.GroupName = value; OnPropertyChanged(); }
    }

    public AddGroupViewModel(NavigationService studentDashboardNavigationService, NavigationService teacherDashboardNavigationService, UserStore userStore, DatabaseService connection)
    {
        _group = new Group(connection);
        _group.GroupCodeGeneratorMethod();

        GroupCodeText = _group.GroupCode.ToString();
        SaveButton = new CreateGroupCommand(_group, userStore.User, connection, teacherDashboardNavigationService);
        
        var teacher = new NavigateCommand(teacherDashboardNavigationService);
        var student = new NavigateCommand(studentDashboardNavigationService);
        BackButton = userStore.User.IsTeacher ? teacher : student;
        
        CancelButton = new CancelCommand(teacherDashboardNavigationService);
        // Groupcode has currently no way to be updated
        //NewGroupCodeButton = new NewGroupCodeCommand(_group, this);
    }
}
