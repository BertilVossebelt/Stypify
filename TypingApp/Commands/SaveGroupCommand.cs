using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using TypingApp.Models;
using TypingApp.Services;
using TypingApp.Stores;
using TypingApp.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TypingApp.Commands;

public class SaveGroupCommand : CommandBase
{
    private readonly User _user;
    private Group _group;
    private DatabaseConnection _connection;
    private NavigationService _teacherDashboardNavigationService;

    public SaveGroupCommand(Group newGroup, User user,DatabaseConnection connection, NavigationService teacherDashboardNavigationService)
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
            string messageBoxText1 = "Je moet een naam invullen";
            string caption1 = "Geen naam";
            MessageBoxButton button1 = MessageBoxButton.OK;
            MessageBoxImage icon1 = MessageBoxImage.Error;
            
            MessageBox.Show(messageBoxText1, caption1, button1, icon1);
        }
        else
        {
            string messageBoxText2 = "Weet je zeker dat je deze groep wilt opslaan";
            string caption2 = "Opslaan";
            MessageBoxButton button2 = MessageBoxButton.YesNo;
            MessageBoxImage icon2 = MessageBoxImage.Question;
            MessageBoxResult result2;

            result2 = MessageBox.Show(messageBoxText2, caption2, button2, icon2);
            if (result2 == MessageBoxResult.Yes)
            {
                //Save here to database
                String QueryString = $"INSERT INTO Groups (teacher_id,name,code) VALUES ({_user.Id},'{_group.GroupName}','{_group.GroupCode}')";
                _connection.ExecuteSqlStatement2(QueryString);

                var navigateCommand = new NavigateCommand(_teacherDashboardNavigationService);
                navigateCommand.Execute(this);
                
                Console.WriteLine(_group.GroupName);
                Console.WriteLine(_group.GroupCode);
            };

        }
    }
}