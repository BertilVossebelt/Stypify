using System.Windows.Input;
using TypingApp.Commands;
using TypingApp.Models;
using TypingApp.Stores;

namespace TypingApp.ViewModels;

public class TeacherDashboardViewModel : ViewModelBase
{
    public ICommand AddGroupButton { get; }

    private string _groupNameText;
    public string GroupNameText 
    {
        get
        {
            return _groupNameText;
        }
        
        set
        {
            _groupNameText = getGroupNameFromDatabase();
            OnPropertyChanged();
        }
    }
    private string _groupCodeText;
    public string GroupCodeText
    {
        get
        {
            return _groupCodeText;
        }

        set
        {
            _groupCodeText = getGroupCodeFromDatabase();

            OnPropertyChanged();
        }
    }

    public TeacherDashboardViewModel(User user, NavigationStore navigationStore)
    {
        GroupNameText = "Placeholder";
        GroupCodeText = "Placeholder";
        AddGroupButton = new AddGroupCommand(user, navigationStore);
    }


    public string getGroupNameFromDatabase()
    {
        //Get from database

        //
        return "testName";
    }
    public string getGroupCodeFromDatabase()
    {
        //Get from database

        //
        return "testCode";
    }

}
