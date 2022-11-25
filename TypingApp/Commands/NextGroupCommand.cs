using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;
using TypingApp.Views;

namespace TypingApp.Commands;

public class NextGroupCommand : CommandBase
{
    private readonly NavigationStore _navigationStore;
    private readonly User _user;
    private readonly DatabaseConnection _connection;
    private readonly TeacherDashboardViewModel _teacherDashboardView;
    
    public NextGroupCommand(User user, NavigationStore navigationStore, DatabaseConnection connection, TeacherDashboardViewModel teacherDashboardViewModel)
    {
        _navigationStore = navigationStore;
        _user = user;
        _connection = connection;
        _teacherDashboardView = teacherDashboardViewModel;
    }

    public override void Execute(object? parameter)
    {
        if(_teacherDashboardView.GroupNumber >= _teacherDashboardView.groupDataArray.Count -1)
        { 
            _teacherDashboardView.GroupNumber = 0; 
        }
        else
        {
            _teacherDashboardView.GroupNumber++;
           
        }
        

        
        _teacherDashboardView.GroupID = _teacherDashboardView.getGroupIDFromDatabase(_teacherDashboardView.GroupNumber);
        _teacherDashboardView.GroupNameText2 = _teacherDashboardView.getGroupNameFromDatabase(_teacherDashboardView.GroupNumber);
        _teacherDashboardView.GroupCodeText2 = _teacherDashboardView.getGroupCodeFromDatabase(_teacherDashboardView.GroupNumber);
        System.Console.WriteLine(_teacherDashboardView.GroupNumber + "ID" + _teacherDashboardView.GroupID);

    }
}