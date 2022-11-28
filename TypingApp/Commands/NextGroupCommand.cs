using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;
using TypingApp.Views;

namespace TypingApp.Commands;

public class NextGroupCommand : CommandBase
{
    private readonly User _user;
    private readonly DatabaseConnection _connection;
    private readonly TeacherDashboardViewModel _teacherDashboardView;
    
    public NextGroupCommand(User user, DatabaseConnection connection, TeacherDashboardViewModel teacherDashboardViewModel)
    {
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



        _teacherDashboardView.GroupID = _teacherDashboardView.GetGroupIdFromDatabase(_teacherDashboardView.GroupNumber);
        _teacherDashboardView.GroupNameText2 = _teacherDashboardView.GetGroupNameFromDatabase(_teacherDashboardView.GroupNumber);
        _teacherDashboardView.GroupCodeText2 = _teacherDashboardView.GetGroupCodeFromDatabase(_teacherDashboardView.GroupNumber);
        System.Console.WriteLine(_teacherDashboardView.GroupNumber + "ID" + _teacherDashboardView.GroupID);

    }
}