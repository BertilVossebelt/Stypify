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
    private Group _group { get; set; }

    private string _groupCodeText { get; set; }
    public string GroupCodeText
    {

        get { return _groupCodeText; }
        set { _groupCodeText = value; OnPropertyChanged(); }
    }

    private string _groupNameText { get; set; }
    public string GroupNameText
    {
        get => _groupNameText;
        set
        {
            _groupNameText = value;
            _group.GroupName = value;
            OnPropertyChanged();
        }
    }

    public AddGroupViewModel(NavigationService studentDashboardNavigationService, NavigationService teacherDashboardNavigationService, User user, DatabaseConnection connection)
    {
        _group = new Group(connection);
        _group.GroupCodeGeneratorMethod();

        GroupCodeText = _group.GroupCode;
        SaveButton = new SaveGroupCommand(_group, user, connection, teacherDashboardNavigationService);
        
        var teacher = new NavigateCommand(teacherDashboardNavigationService);
        var student = new NavigateCommand(studentDashboardNavigationService);
        BackButton = user.IsTeacher ? teacher : student;
        
        CancelButton = new CancelCommand(teacherDashboardNavigationService, user, connection);
        NewGroupCodeButton = new NewGroupCodeCommand(_group, user ,connection,this);
    }
}
