using System;
using TypingApp.Models;
using TypingApp.ViewModels;

namespace TypingApp.Commands;
internal class UpdateGroupCodeCommand : CommandBase
    {
        private Group _group;
        private readonly DatabaseConnection _connection;
        private readonly TeacherDashboardViewModel _teacherDashboardViewModel;
        public UpdateGroupCodeCommand(Group CurrentGroup, DatabaseConnection connection,TeacherDashboardViewModel teacherDashboardViewModel)
        {
            _group = CurrentGroup;
            _connection = connection;
            _teacherDashboardViewModel = teacherDashboardViewModel;
        }

        public override void Execute(object? parameter)
        {
            _group.GroupCodeGeneratorMethod();

            var queryString3 = $"UPDATE Groups SET code='{_group.GroupCode}'WHERE id='{_group.GroupId}'";
            _connection.ExecuteSqlStatement2(queryString3);
            
            _teacherDashboardViewModel.GetGroupsFromDatabase();
            _teacherDashboardViewModel.GroupCodeText = _teacherDashboardViewModel.GetGroupCodeFromDatabase(_teacherDashboardViewModel.GroupNumber);
        }
    }



