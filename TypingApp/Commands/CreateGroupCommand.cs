using System;
using System.Windows;
using TypingApp.Models;
using TypingApp.Services;

namespace TypingApp.Commands;

public class CreateGroupCommand : CommandBase
{
    private readonly User _user;
    private Group _group;
    private DatabaseService _connection;
    private NavigationService _teacherDashboardNavigationService;

    public CreateGroupCommand(Group newGroup, User user, DatabaseService connection, NavigationService teacherDashboardNavigationService)
    {
        _group = newGroup;
        _user = user;
        _connection = connection;
        _teacherDashboardNavigationService = teacherDashboardNavigationService;
    }

    public override void Execute(object? parameter)
    {
        if(_group.GroupName == "" || _group.GroupName == null)
        {
            MessageBox.Show("Je moet een naam invullen", "Geen naam", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        else
        {
            var SaveMessageBox = MessageBox.Show("Weet je zeker dat je deze groep wilt opslaan", "Opslaan", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (SaveMessageBox == MessageBoxResult.Yes)
            {
                //Save here to database
                String QueryString = $"INSERT INTO Groups (teacher_id,name,code) VALUES ({_user.Id},'{_group.GroupName}','{_group.GroupCode}')";
                _connection.ExecuteSqlStatement2(QueryString);

                var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
                navigateCommand.Execute(this);
            };
        }
    }
}