using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
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

    public AddGroupViewModel(Group newGroup,NavigationStore navigationStore, User user,DatabaseConnection connection)
    {
        _group = newGroup;

        GroupCodeText = newGroup.GroupCode;
        SaveButton = new SaveGroupCommand(newGroup,navigationStore, user, connection);
        BackButton = new BackCommand(navigationStore, user, connection);
        CancelButton = new CancelCommand(navigationStore, user, connection);
        NewGroupCodeButton = new NewGroupCodeCommand(newGroup, navigationStore, user ,connection,this);
    }
}
