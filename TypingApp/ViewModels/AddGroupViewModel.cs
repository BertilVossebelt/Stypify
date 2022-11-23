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
        get => _groupCodeText;
        set
        {
            _groupCodeText = _group.GroupCode;
            OnPropertyChanged();
        }
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


    public AddGroupViewModel(Group newGroup,NavigationStore navigationStore, User user)
    {
        _group = newGroup;
        GroupCodeText = newGroup.GroupCode;
        SaveButton = new SaveGroupCommand(newGroup,navigationStore, user);
        BackButton = new BackCommand(navigationStore, user);
        CancelButton = new CancelCommand(navigationStore, user);
        NewGroupCodeButton = new NewGroupCodeCommand(newGroup, navigationStore, user);
    }
}
