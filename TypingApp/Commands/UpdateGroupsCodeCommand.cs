using TypingApp.Services;
using TypingApp.ViewModels;

namespace TypingApp.Commands;

public class UpdateGroupsCodeCommand : CommandBase
{
    private readonly TeacherDashboardViewModel _teacherDashboardViewModel;
    private readonly AddGroupViewModel _addGroupViewModel;

    /*
     * For usage on the TeacherDashboardViewModel.
     */
    public UpdateGroupsCodeCommand(TeacherDashboardViewModel dash)
    {
        _teacherDashboardViewModel = dash;
    }

    /*
     * For usage on the AddGroupViewModel.
     */
    public UpdateGroupsCodeCommand(AddGroupViewModel dash)
    {
        _addGroupViewModel = dash;
    }

    /*
     * This method is used to update the code of a group.
     * --------------------------------------------------
     * Method should only be used for teachers.
     */
    public override void Execute(object? parameter)
    {
        if (_teacherDashboardViewModel != null)
        {
            if (_teacherDashboardViewModel.SelectedItem is null) return;
            _teacherDashboardViewModel.SelectedItem.RefreshCode();
            _teacherDashboardViewModel.SelectedItem = _teacherDashboardViewModel.SelectedItem;
        }
        else
        {
            _addGroupViewModel.GroupCode = new GroupCodeService().GenerateCode();
        }
    }
}