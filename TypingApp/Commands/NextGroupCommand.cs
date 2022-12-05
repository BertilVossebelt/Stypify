using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class NextGroupCommand : CommandBase
{
    private readonly TeacherDashboardViewModel _teacherDashboardView;

    public NextGroupCommand(TeacherDashboardViewModel teacherDashboardViewModel)
    {
        _teacherDashboardView = teacherDashboardViewModel;
    }

    public override void Execute(object? parameter)
    {
        if (_teacherDashboardView.GroupNumber >= _teacherDashboardView.groupDataArray.Count - 1)
        {
            _teacherDashboardView.GroupNumber = 0;
        }
        else
        {
            _teacherDashboardView.GroupNumber++;
        }
        _teacherDashboardView.GroupID = _teacherDashboardView.GetGroupIdFromDatabase(_teacherDashboardView.GroupNumber);
        _teacherDashboardView.GroupNameText = _teacherDashboardView.GetGroupNameFromDatabase(_teacherDashboardView.GroupNumber);
        _teacherDashboardView.GroupCodeText = _teacherDashboardView.GetGroupCodeFromDatabase(_teacherDashboardView.GroupNumber);
    }
}