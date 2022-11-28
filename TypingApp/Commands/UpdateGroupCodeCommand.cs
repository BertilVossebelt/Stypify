using System;
using System.Windows.Controls;
using System.Windows.Data;
using TypingApp.Models;
using TypingApp.Stores;
using TypingApp.ViewModels;
using TypingApp.Views;

namespace TypingApp.Commands
{
    internal class UpdateGroupCodeCommand : CommandBase
    {
        private readonly User _user;
        private Group _group;
        private readonly DatabaseConnection _connection;
        private readonly TeacherDashboardViewModel _teacherDashboardViewModel;
        public UpdateGroupCodeCommand(Group CurrentGroup, User user, DatabaseConnection connection,TeacherDashboardViewModel teacherDashboardViewModel)
        {
            _group = CurrentGroup;
            _user = user;
            _connection = connection;
            _teacherDashboardViewModel = teacherDashboardViewModel;
        }

        public override void Execute(object? parameter)
        {

            _group.GroupCodeGeneratorMethod();

            System.Console.WriteLine("Nieuwe code: "+ _group.GroupCode);
            string QueryString3 = $"UPDATE Groups SET code='{_group.GroupCode}'WHERE id='{_group.GroupID}'";
            _connection.ExecuteSqlStatement2(QueryString3);

            _teacherDashboardViewModel.GetGroupsFromDatabase();
            _teacherDashboardViewModel.GroupCodeText2 = _teacherDashboardViewModel.GetGroupCodeFromDatabase(_teacherDashboardViewModel.GroupNumber);
        }
    }

}

